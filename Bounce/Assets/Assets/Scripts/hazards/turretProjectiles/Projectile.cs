﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private Vector3 dir;
	private IFirable parent;
	public float damageToDeal;
	public float speed;
	public float acceleration;
	public float lifeTime;
	private float timeGoneBy;
	[SerializeField]
	protected float hitRadius;
	[SerializeField]
	protected string[] layersToHarm;

	// Start is called before the first frame update
	void Awake()
	{

	}

	// Update is called once per frame
	void Update()
	{
		speed = speed + acceleration * timeGoneBy;
		transform.position += dir * (speed * Time.deltaTime);

		timeGoneBy += Time.deltaTime;

		if (timeGoneBy >= lifeTime)
		{
			Destroy(gameObject);
		}

		CollisionDetect();
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{

	}

	public void OnDestroy()
	{
		parent.BulletCallback();
	}

	private void CollisionDetect()
	{
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, hitRadius, Vector2.zero);
		bool destroy = false;
		foreach (RaycastHit2D hit in hits)
		{
			foreach (string layerName in layersToHarm)
			{
				if (LayerMask.LayerToName(hit.collider.gameObject.layer) == layerName)
				{
					//Destoryable destoryable = hit.collider.gameObject.GetComponent<Destoryable>();
					//if (destoryable != null)
					//{
					//	destoryable.DealDamage(damageToDeal);
					//	destroy = true;
					//}
					Debug.Log("bang");
				}
			}

			if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Wall")
			{
				destroy = true;
			}
		}

		if (destroy)
		{
			Destroy(gameObject);
		}
	}

	public Vector3 Direction
    {
        set
        {
            dir = value;
        }
    }
    public IFirable Parent
    {
        get
        {
            return parent;
        }

        set
        {
            parent = value;
        }
    }

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, hitRadius);
	}
}
