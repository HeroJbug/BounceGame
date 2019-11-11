using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEnemy : Enemy
{
    public bool inHazardMode;
    public float modeSwapTimer = 10f;
    private float modeSwapInternal;
    private float moveSpeedInternal;
    private Animator anim;
    public float hazardModeRadius;
    public float hazardModeDamage;
    private float pathRecalc;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSelf();
        inHazardMode = false;
        modeSwapInternal = modeSwapTimer;
        moveSpeedInternal = speed;
        anim = GetComponent<Animator>();
        RequestNewPath();
        pathRecalc = 0.75f;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!inHazardMode)
        {
            base.Update();
        }
        else
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, hazardModeRadius, Vector2.right, 0, LayerMask.GetMask("Player"));
            if (hits.Length != 0)
            {
                foreach (RaycastHit2D hit in hits)
                    if (hit.rigidbody.gameObject.GetComponent<PlayerCollision>() != null)
                        hit.rigidbody.gameObject.GetComponent<PlayerCollision>().TakeDamage(hazardModeDamage);
            }
        }

        if(modeSwapInternal <= 0)
        {
            ModeSwap();
            modeSwapInternal = modeSwapTimer;
        }
        else
        {
            modeSwapInternal -= Time.deltaTime;
        }

        if (pathRecalc <= 0)
        {
            if (!GetInKnockback())
                RequestNewPath();
            pathRecalc = 0.75f;
        }
        else
        {
            pathRecalc -= Time.deltaTime;
        }
    }

    private void ModeSwap()
    {
        if(inHazardMode)
        {
            rbody.isKinematic = false;
            speed = moveSpeedInternal;
            anim.SetBool("hazardMode", false);
            inHazardMode = false;
        }
        else
        {
            rbody.isKinematic = true;
            speed = 0f;
            anim.SetBool("hazardMode", true);
            inHazardMode = true;
        }
    }
}
