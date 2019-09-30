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
				if (ColInCircleAll(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit[] hits))
				{
					foreach (RaycastHit hit in hits)
					{
						//deal damage by damageToDeal
						Debug.Log("Yaboi!");
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
