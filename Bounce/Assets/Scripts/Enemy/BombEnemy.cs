using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : Enemy
{
    public float countdownTimer = 5f;
    private SpriteRenderer renderer;
    public float radius = 10f;
    public float damage = 2f;
    public bool drawGizmos;

    private void Start()
    {
        base.InitializeSelf();
        renderer = GetComponent<SpriteRenderer>();
    }

    public override void Update()
    {
        base.Update();
        if(countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            TintSprite();
            if(!GetInKnockback())
                RequestNewPath();
        }
        else
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        rbody.velocity = Vector2.zero;
        StopCoroutine("FollowPath");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.right, 0, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            foreach(RaycastHit2D hit in hits)
                if(hit.rigidbody.gameObject.GetComponent<PlayerCollision>() != null)
                    hit.rigidbody.gameObject.GetComponent<PlayerCollision>().TakeDamage(damage);
        }
        //play animation here
        GetComponent<Animator>().SetTrigger("OnExplode");
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }



    private void TintSprite()
    {
        renderer.color = new Color(1f, countdownTimer / 5f, countdownTimer / 5f);
    }

    public void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
