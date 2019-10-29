using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private float recalculatePathTimer = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        InitializeSelf();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (recalculatePathTimer <= 0)
        {
            RequestNewPath();
            recalculatePathTimer = 0.5f;
        }
        else
        {
            recalculatePathTimer -= Time.deltaTime;
        }
    }
}
