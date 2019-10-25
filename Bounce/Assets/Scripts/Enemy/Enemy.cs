using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float turnSpeed = 3f;
    public float turnDst = 5;
    public bool debugPath;
    Path path;
    //Vector3[] path;
    //int targetIdx;
    [SerializeField]
    private bool inKnockback;
    private float recalculatePathTimer = 0.2f;
    private Rigidbody2D rbody;
    Vector2 threshold;
	public ParticleSystem explosion;

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

    public void PathFound(Vector3[] waypoints, bool pathSuccess)
    {
        if(pathSuccess)
        {
            path = new Path(waypoints, transform.position, turnDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIdx = 0;
        Vector3 currentWP = path.lookPoints[0];
        int targetIdx = 0;
        transform.right = target.position - transform.position;
        while (followingPath)
        {
            if (transform.position == currentWP)
            {
                targetIdx++;
                if (targetIdx >= path.lookPoints.Length)
                {
                    yield break;
                }
                currentWP = path.lookPoints[targetIdx];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWP, speed * Time.deltaTime);

            //SMMOTHED CALCULATION
            //Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            //if(path.turnBoundaries[pathIdx].HasCrossedLine(pos))
            //{
            //    if(pathIdx == path.finishLineIdx)
            //    {
            //        followingPath = false;
            //    }
            //    else
            //    {
            //        pathIdx++;
            //    }
            //}

            //if(followingPath)
            //{
            //    Quaternion targetRot = Quaternion.LookRotation(path.lookPoints[pathIdx] - transform.position);
            //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
            //    transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            //}
            yield return null;

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

	public void OnCollisionDeath()
	{
		Instantiate(explosion, transform.position, Quaternion.identity).transform.Rotate(new Vector3(180, 0, 0));
		Destroy(this.gameObject);
	}

	public void OnDestroy()
	{
		this.StopAllCoroutines();
	}

	private float CalcDstToPlayer()
    {
        return Vector2.Distance(this.transform.position, target.transform.position);
    }

    private void OnDrawGizmos()
    {
        if(path != null && debugPath)
        {
            path.GizmoDebugger();
        }
    }
}
