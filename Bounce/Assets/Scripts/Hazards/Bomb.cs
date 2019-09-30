using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Hazard
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
		type = HazardTypes.BOMB;
    }

    // Update is called once per frame
    void Update()
    {
        if (Landed)
		{
			foreach(string layerName in layersToAffect)
			{
				if (ColInCircleAll(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit[] hits))
				{
					foreach(RaycastHit hit in hits)
					{
						//damage player by damageToDeal
						Debug.Log("KAAAAAAAAA-BOOOOOOOOOOOOOMMMMM!!!!!!!!!!!!!!!!");
					}
				}
				Destroy(this.gameObject);
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
