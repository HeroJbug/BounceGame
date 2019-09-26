using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed;
    Vector3[] path;
    int targetIdx;

    private float recalculatePathTimer = 0.5f;

    private void Update()
    {
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

    private void Start()
    {
        Physics.IgnoreLayerCollision(10,10);
        EnemyPathRequestManager.RequestPath(transform.position, target.position, PathFound);
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
}
