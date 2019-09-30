using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float enemyKnockBackForce = 25f;
    public float enemyDamage = 1f;

    PlayerMovement moveRef;
    // Start is called before the first frame update
    void Start()
    {
        moveRef = this.GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ContactPoint hitPoint = collision.GetContact(0);
            if (moveRef.PlayerIsBoosting())
                collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(enemyKnockBackForce, transform.position - hitPoint.normal, 5f, 0f, ForceMode.Impulse);
            else
            {
                TakeDamage(enemyDamage);
                Destroy(collision.gameObject);
            }
        }
    }

    public void TakeDamage(float amt)
    {
        //TODO: implement
        print("ouch");
    }
}

