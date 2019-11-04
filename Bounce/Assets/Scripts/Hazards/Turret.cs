﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Hazard, IFirable
{
	[SerializeField]
	protected Projectile projectilePrefab;
	[SerializeField]
	protected Vector2 bulletOffset;
	[SerializeField]
	protected int maxShots;
	[SerializeField]
	protected float shotCooldown;
	protected int shots;
	protected float cooldown;
	[SerializeField]
	protected GameObject turretBase;
	[SerializeField]
	protected bool drawGizmos = true;

	// Start is called before the first frame update
	void Awake()
    {
		Initialize();
		type = HazardTypes.TURRET;
		shots = maxShots;
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
				if (cooldown <= 0 && shots > 0 && ColInCircle(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit2D hit))
				{
					Vector3 firDir = hit.transform.position - this.transform.position;
					firDir.Normalize();

					float newZrot = Vector2.SignedAngle(Vector2.right, firDir);
					transform.eulerAngles = Vector3.forward * newZrot;

					Projectile proj = Instantiate<Projectile>(projectilePrefab, transform.position + (Vector3)bulletOffset + Vector3.up * -1.57f, Quaternion.identity);
					proj.Direction = firDir;
					proj.Parent = this;

					cooldown = shots > 0 ? shotCooldown : 4 * shotCooldown;
					shots--;
				}
			}

			if (cooldown > 0)
			{
				cooldown -= Time.deltaTime;
			}
		}
		else
		{
			Falling();
		}

		turretBase.transform.eulerAngles = Vector3.zero;
	}

	public void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, targetRadius);
		}
	}

	public void BulletCallback()
	{
		shots++;
	}
}
