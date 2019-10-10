using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    Vector3[] path;
    int targetIdx;
    [SerializeField]
    private bool inKnockback;
    private float recalculatePathTimer = 0.2f;
    private Rigidbody2D rbody;
    Vector2 threshold;

    private void Start()
    {
        EnemyPathRequestManager.RequestPath(transform.position, target.position, PathFound);
        inKnockback = false;
        rbody = GetComponent<Rigidbody2D>();
        threshold = new Vector2(13f, 13f);
    }


    private void Update()
    {
        if(inKnockback)
        {
            if(Mathf.Abs(rbody.velocity.magnitude) < Mathf.Abs(threshold.magnitude))
            {
                inKnockback = false;
            }
        }
        if(recalculatePathTimer <= 0)
        {
            EnemyPathRequestManager.RequestPath(transform.position, target.position, PathFound);
            recalculatePathTimer = 0.5f;
        }
        else
        {
            recalculatePathTimer -= Time.deltaTime;
        }
    }

    public void PathFound(Vector3[] newPath, bool pathSuccess)
    {
        if(pathSuccess)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWP = path[0];
            while (true)
            {
                if (transform.position == currentWP)
                {
                    targetIdx++;
                    if (targetIdx >= path.Length)
                    {
                        yield break;
                    }
                    currentWP = path[targetIdx];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWP, speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!inKnockback)
        {
            Physics2D.IgnoreLayerCollision(10, 10);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(10, 10, false);
        }
    }

    public void SetInKnockback(bool _ik)
    {
        inKnockback = _ik;
    }

    public bool GetInKnockback()
    {
        return inKnockback;
    }

    public float ApplyAdditiveKnockback(float modifierAmt, Vector2 moveDir)
    {
        if(modifierAmt > 0.1f)
        {
            rbody.AddForce(modifierAmt * -moveDir, ForceMode2D.Impulse);
            inKnockback = true;
            return modifierAmt * 0.5f;
        }
        return 0;
    }

    private float CalcDstToPlayer()
    {
        return Vector2.Distance(this.transform.position, target.transform.position);
    }
}
