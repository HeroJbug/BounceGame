using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : Enemy
{
    private Animator anim;
    public float radius = 10f;
    public float damage = 2f;
    public bool drawGizmos;
    private float countdown = 1f;
    private bool startCountdown = false;

    private void Start()
    {
        base.InitializeSelf();
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        if(!CheckForExplodeTrigger() && !startCountdown)
        {
            if(!GetInKnockback())
                RequestNewPath();
        }
        else
        {
            startCountdown = true;
            StopCoroutine("FollowPath");
            anim.SetTrigger("StartFuse");
        }

        if(startCountdown)
        {
            countdown -= Time.deltaTime;
            if(countdown <= 0)
                StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        rbody.velocity = Vector2.zero;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.right, 0, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            foreach(RaycastHit2D hit in hits)
                if(hit.rigidbody.gameObject.GetComponent<PlayerCollision>() != null)
                    hit.rigidbody.gameObject.GetComponent<PlayerCollision>().TakeDamage(damage, false);
        }
        //play animation here
        anim.SetTrigger("OnExplode");
        yield return new WaitForSeconds(0.5f);

        base.OnCollisionDeath();
    }

    private bool CheckForExplodeTrigger()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.right, 0, LayerMask.GetMask("Player"));
        if (hits.Length != 0)
        {
            foreach (RaycastHit2D hit in hits)
                if (hit.rigidbody.gameObject.GetComponent<PlayerCollision>() != null)
                    return true;
        }
        return false;
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
