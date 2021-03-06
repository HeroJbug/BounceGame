﻿using System.Collections;
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
            timer -= Time.deltaTime;
            if (timer <= 0)
                Destroy(this.gameObject);
            foreach (string layerName in layersToAffect)
			{
				if (ColInCircleAll(new Vector3(transform.position.x - 2.85f, transform.position.y-0.3f, transform.position.z), targetRadius, LayerMask.GetMask(layerName), out RaycastHit2D[] hits))
				{
					foreach (RaycastHit2D hit in hits)
					{
						//damage player by damageToDeal
						if (hit.rigidbody.gameObject.GetComponent<PlayerCollision>() != null)
						{
							hit.rigidbody.gameObject.GetComponent<PlayerCollision>().TakeDamage(damageToDeal, false);
						}
						//destroy enemies enemies
						if (hit.rigidbody.gameObject.GetComponent<Enemy>() != null)
						{
							hit.rigidbody.gameObject.GetComponent<Enemy>().OnCollisionDeath();
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
			Gizmos.DrawWireSphere(new Vector3(transform.position.x - 2.85f, transform.position.y-0.3f, transform.position.z), targetRadius);
		}
	}
}
