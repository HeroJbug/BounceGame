using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Hazard
{
	/// <summary>
	/// The damage to deal to entity
	/// </summary>
	public float damageToDeal;
	[SerializeField]
	protected bool drawGizmos = true;
	private List<RaycastHit> prevHits;

    // Start is called before the first frame update
    void Awake()
    {
		Initialize();
		type = HazardTypes.SPIKES;
    }

    // Update is called once per frame
    void Update()
    {
		if (Landed) //if the hazard has landed
		{
			foreach (string layerName in layersToAffect)
			{
				if (ColInCircleAll(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit2D[] hits))
				{
					foreach (RaycastHit2D hit in hits)
					{
						//damage player by damageToDeal
						if (hit.rigidbody.gameObject.GetComponent<PlayerCollision>() != null)
						{
							hit.rigidbody.gameObject.GetComponent<PlayerCollision>().TakeDamage(damageToDeal);
						}
						//destroy enemies enemies
						if (hit.rigidbody.gameObject.GetComponent<EnemyCollision>() != null)
						{
							hit.rigidbody.gameObject.GetComponent<EnemyCollision>().TakeDamage(10f, hit.rigidbody.transform.position);
						}
					}
				}
			}
		}
		else //if the hazard is falling down
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
