using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created in Advance to handle enemy collision with various hazards
public class EnemyCollision : MonoBehaviour
{
    public float health = 1f;
    public ParticleSystem explosion;

    private void Start()
    {
    }

    public void TakeDamage(float amt, Vector3 colPoint)
    {
        health -= amt;
        if(health <= 0)
        {
            OnDeath(colPoint);
        }
    }

    private void OnDeath(Vector3 collidePt)
    {
        Instantiate(explosion, collidePt, Quaternion.identity).transform.Rotate(new Vector3(180, 0, 0));
        Destroy(this.gameObject);
    }

}
