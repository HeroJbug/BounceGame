using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricHazard : Hazard
{
    public float damageToDeal;
    [SerializeField]
    private BoxCollider2D triggerCollider;
    [SerializeField]
    private GameObject electricity;

    private void Awake()
    {
        Initialize();
        type = HazardTypes.ELECTRIC;
    }

    private void Update()
    {
        if (Landed) //if the hazard has landed
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                Destroy(this.gameObject);
            if(triggerCollider.enabled == false)
            {
                triggerCollider.enabled = true;
                electricity.SetActive(true);
            }
        }
        else //if the hazard is falling down
        {
            Falling();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hit = collision.gameObject;
        if (hit.GetComponent<PlayerCollision>() != null)
        {
            hit.GetComponent<PlayerCollision>().TakeDamage(damageToDeal, true);
        }
        //destroy enemies enemies
        if (hit.GetComponent<Enemy>() != null)
        {
            hit.GetComponent<Enemy>().OnCollisionDeath();
        }
    }
}
