using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Hazard
{
    protected bool drawGizmos = true;
    BoxCollider2D collider;

    private void Awake()
    {
        Initialize();
        type = HazardTypes.BARRIER;
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //if we landed, check everything under us and just kill it
        if(Landed)
        {
            collider.enabled = true;
            timer -= Time.deltaTime;
            if (timer <= 0)
                Destroy(this.gameObject);
            foreach (string layerName in layersToAffect)
            {
                if (ColInCircleAll(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit2D[] hits))
                {
                    foreach (RaycastHit2D hit in hits)
                    {
                        //damage player by some giant number to make sure they real dead
                        if (hit.rigidbody.gameObject.GetComponent<PlayerCollision>() != null)
                        {
                            hit.rigidbody.gameObject.GetComponent<PlayerCollision>().TakeDamage(1000000000f, false);
                        }
                        //destroy enemies
                        if (hit.rigidbody.gameObject.GetComponent<Enemy>() != null)
                        {
							Destroy(hit.rigidbody.gameObject.GetComponent<Enemy>().gameObject);
						}
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
