using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created in Advance to handle enemy collision with various hazards
public class EnemyCollision : MonoBehaviour
{
    public float health = 1f;
    public float additiveKnockbackDef = 3.0f;
    public float additiveKnockback;
    public ParticleSystem explosion;

    private void Start()
    {
        additiveKnockback = additiveKnockbackDef;
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
		//ScoreSystem.system.IncrementScore();
		Debug.Log("OnDeath");
		Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if(this.gameObject.GetComponentInParent<Enemy>().GetInKnockback())
            {
                Vector2 moveDir = this.transform.position - collision.gameObject.GetComponent<Rigidbody2D>().transform.position;
                //assign to other's additive knockback so it keeps getting halved
                //TODO: i'm a touch buggy atm
                collision.gameObject.GetComponentInChildren<EnemyCollision>().additiveKnockback = collision.gameObject.GetComponent<Enemy>().ApplyAdditiveKnockback(additiveKnockbackDef, moveDir);
            }
        }
    }

}
