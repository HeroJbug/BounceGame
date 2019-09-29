using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : Hazard
{
	[SerializeField]
	protected float acceleration;
	[SerializeField]
	protected bool drawGizmos = true;
	protected Dictionary<PlayerMovement, float> defaultSpeeds;
	protected List<PlayerMovement> lastDetectedPlayers;

	// Start is called before the first frame update
	void Awake()
    {
		Initialize();
		type = HazardTypes.OILSPILL;
		defaultSpeeds = new Dictionary<PlayerMovement, float>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Landed)
		{
			foreach(string layerName in layersToAffect)
			{
				List<PlayerMovement> detectedPlayers = new List<PlayerMovement>();
				if (ColInCircleAll(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit[] hits))
				{
					foreach (RaycastHit hit in hits)
					{
						PlayerMovement player = hit.collider.gameObject.GetComponent<PlayerMovement>();
						detectedPlayers.Add(player);
						if (!defaultSpeeds.ContainsKey(player)) { defaultSpeeds.Add(player, player.speed); }

						player.speed += acceleration;
						
					}
				}

				if (detectedPlayers != lastDetectedPlayers)
				{
					if (lastDetectedPlayers != null)
					{
						if (detectedPlayers == null)
						{
							foreach(PlayerMovement player in lastDetectedPlayers)
							{
								player.speed = defaultSpeeds[player];
							}
						}
						else
						{
							foreach(PlayerMovement player in lastDetectedPlayers)
							{
								if (!detectedPlayers.Contains(player))
								{
									player.speed = defaultSpeeds[player];
								}
							}
						}
					}
					else
					{
						lastDetectedPlayers = detectedPlayers;
					}
				}
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
