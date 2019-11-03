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
    [SerializeField]
    private Rigidbody2D rbody;
    Vector2 threshold;
	public ParticleSystem explosion;

    private void Start()
    {
        InitializeSelf();
    }

    public void InitializeSelf()
    {
        if(target != null)
            EnemyPathRequestManager.RequestPath(transform.position, target.position, PathFound);
        inKnockback = false;
        rbody = GetComponent<Rigidbody2D>();
        threshold = new Vector2(13f, 13f);
    }


    public virtual void Update()
    {
        if(inKnockback)
        {
            if(Mathf.Abs(rbody.velocity.magnitude) < Mathf.Abs(threshold.magnitude))
            {
                inKnockback = false;
            }
        }
    }

    public void PathFound(Vector3[] waypoints, bool pathSuccess)
    {
        if (pathSuccess)
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
        //transform.right = target.position - transform.position;
        while (followingPath)
        {
            //SMOOTHED CALCULATION
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            if (pathIdx < path.turnBoundaries.Length && path.turnBoundaries[pathIdx].HasCrossedLine(pos))
            {
                if (pathIdx == path.finishLineIdx)
                {
                    followingPath = false;
                }
                else
                {
                    pathIdx++;
                }
            }

            if (followingPath && pathIdx < path.lookPoints.Length)
            {
                //float angle =Mathf.Atan2(path.lookPoints[pathIdx].y - transform.position.y, path.lookPoints[pathIdx].x - transform.position.x);
                //angle *= Mathf.Rad2Deg;
                //Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
                //transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
                transform.position = Vector3.MoveTowards(transform.position, path.lookPoints[pathIdx], speed * Time.deltaTime);
            }
            yield return null;

        }
    }

    protected void RequestNewPath()
    {
        EnemyPathRequestManager.RequestPath(transform.position, target.position, PathFound);
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
