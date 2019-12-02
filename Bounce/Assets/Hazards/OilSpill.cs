using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : Hazard
{
	[SerializeField]
	[Range(0, 1)]
	protected float acceleration;
	[SerializeField]
	protected bool drawGizmos = true;
	protected Dictionary<PlayerMovement, Vector2> forceDirs;
	protected List<PlayerMovement> lastDetectedPlayers;

	// Start is called before the first frame update
	void Awake()
    {
		Initialize();
		type = HazardTypes.OILSPILL;
		forceDirs = new Dictionary<PlayerMovement, Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Landed)
		{
            timer -= Time.deltaTime;
            if (timer <= 0)
                Destroy(this.gameObject);
            foreach (string layerName in layersToAffect)
			{
				List<PlayerMovement> detectedPlayers = new List<PlayerMovement>();
				if (ColInCircleAll(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit2D[] hits))
				{
					//Debug.Log("is in");
					foreach (RaycastHit2D hit in hits)
					{
						PlayerMovement player = hit.collider.gameObject.GetComponent<PlayerMovement>();
						detectedPlayers.Add(player);
						Vector2 dir;

						if (!forceDirs.ContainsKey(player))
						{
							forceDirs.Add(player, Vector3.zero);
						}

						if (!player.PlayerIsBoosting() && player.MoveVector != Vector3.zero)
						{
							dir = player.MoveVector;
							dir.Normalize();

							forceDirs[player] += dir;
							forceDirs[player].Normalize();
							player.slipVec = (Vector3)(forceDirs[player]);
							player.slipSpeed += acceleration;
						}
						else if (player.PlayerIsBoosting())
						{
							dir = player.boostDir;
							dir.Normalize();

							forceDirs[player] += dir;
							forceDirs[player].Normalize();
							player.slipVec = (Vector3)(forceDirs[player]);
							player.slipSpeed += 4 * acceleration;
						}
					}
				}

				if (lastDetectedPlayers != null)
				{
					if (detectedPlayers == null)
					{
						foreach (PlayerMovement player in lastDetectedPlayers)
						{
							forceDirs.Remove(player);
						}
					}
					else
					{
						foreach (PlayerMovement player in lastDetectedPlayers)
						{
							if (!detectedPlayers.Contains(player))
							{
								forceDirs.Remove(player);
							}
						}
					}

				}
				lastDetectedPlayers = detectedPlayers;
			}
		}
		else
		{
			Falling();
		}
	}

	public void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, targetRadius);
		}
	}
}
