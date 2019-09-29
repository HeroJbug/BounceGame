using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : Hazard
{
	[SerializeField]
	protected bool drawGizmos = true;

	// Start is called before the first frame update
	void Awake()
    {
		Initialize();
		type = HazardTypes.OILSPILL;
    }

    // Update is called once per frame
    void Update()
    {
        if (Landed)
		{
			foreach(string layerName in layersToAffect)
			{
				if (ColInCircleAll(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit2D[] hits))
				{
					foreach (RaycastHit2D hit in hits)
					{
						float distance = hit.distance;
						float velocity = distance / Time.deltaTime;
						
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
