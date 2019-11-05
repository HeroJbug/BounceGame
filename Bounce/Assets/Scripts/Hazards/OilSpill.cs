using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : Hazard
{
	[SerializeField]
	protected float acceleration;
	[SerializeField]
	protected bool drawGizmos = true;
	protected Dictionary<PlayerMovement, float> currentSlipSpeed;
	protected Dictionary<PlayerMovement, Vector2> forceDirs;
	protected List<PlayerMovement> lastDetectedPlayers;

	// Start is called before the first frame update
	void Awake()
    {
		Initialize();
		type = HazardTypes.OILSPILL;
		currentSlipSpeed = new Dictionary<PlayerMovement, float>();
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
						if (!currentSlipSpeed.ContainsKey(player))
						{
							currentSlipSpeed.Add(player, player.PlayerIsBoosting() ? player.speed : player.boostSpeed);
							Vector2 distance = hit.point - (Vector2)transform.position;
							distance.Normalize();
							distance *= -1;
							forceDirs.Add(player, distance);
						}

						currentSlipSpeed[player] += acceleration;

						player.SlipVec = (Vector3)(forceDirs[player] * acceleration);

						//player.GetComponent<Rigidbody2D>().AddForce(distance * (acceleration * player.GetComponent<Rigidbody2D>().mass));
					}
				}

				if (lastDetectedPlayers != null)
				{
					if (detectedPlayers == null)
					{
						foreach (PlayerMovement player in lastDetectedPlayers)
						{
							currentSlipSpeed.Remove(player);
							player.SlipVec = Vector3.zero;
						}
					}
					else
					{
						foreach (PlayerMovement player in lastDetectedPlayers)
						{
							if (!detectedPlayers.Contains(player))
							{
								currentSlipSpeed.Remove(player);
								forceDirs.Remove(player);
								player.SlipVec = Vector3.zero;
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
