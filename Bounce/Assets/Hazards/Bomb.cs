﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Hazard
{
	/// <summary>
	/// The damage to deal to entity
	/// </summary>
	public float damageToDeal;
    [SerializeField]
    protected float countdownTimer;
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
            timer -= Time.deltaTime;
            if (timer <= 0)
                Destroy(this.gameObject);
            countdownTimer -= Time.deltaTime;
            if(countdownTimer <= 0)
            {
                foreach (string layerName in layersToAffect)
                {
                    if (ColInCircleAll(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit2D[] hits))
                    {
                        foreach (RaycastHit2D hit in hits)
                        {
                            //TODO: is there a cleaner way to do this?
                            //damage player by damageToDeal
                            //Debug.Log("KAAAAAAAAA-BOOOOOOOOOOOOOMMMMM!!!!!!!!!!!!!!!!");
                            if(hit.rigidbody.gameObject.GetComponent<PlayerCollision>() != null)
                            {
                                hit.rigidbody.gameObject.GetComponent<PlayerCollision>().TakeDamage(damageToDeal, false);
                            }
                            //esplode enemies
                            if(hit.rigidbody.gameObject.GetComponent<Enemy>() != null)
                            {
                                print("boom enemy");
								hit.rigidbody.gameObject.GetComponent<Enemy>().OnCollisionDeath();
							}
                        }
                    }
                    StartCoroutine(Explode());
                }
            }	
		}
		else
		{
			Falling();
		}
    }

    IEnumerator Explode()
    {
        GetComponent<Animator>().SetTrigger("OnExplode");
        transform.localScale = new Vector3(3f, 3f, 1f);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
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
