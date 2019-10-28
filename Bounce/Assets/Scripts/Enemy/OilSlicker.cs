using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlicker : Enemy
{
    public GameObject oilSlick;
    public float oilSlickSpawnTimer = 5f;
    private float oilSpawnTimerInternal;
    private GameObject prevTarget;

    // Start is called before the first frame update
    void Start()
    {
        base.InitializeSelf();
        oilSpawnTimerInternal = 0f;
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        if(oilSpawnTimerInternal <= 0)
        {
            CreateOilSlick();
            oilSpawnTimerInternal = oilSlickSpawnTimer;
            target = PickNewRandomTargetLocation();
        }
        else
        {
            oilSpawnTimerInternal -= Time.deltaTime;
        }
    }

    private Transform PickNewRandomTargetLocation()
    {
        if (prevTarget != null)
            Destroy(prevTarget);

        float x = Random.Range(-55, 55);
        float y = Random.Range(-55, 55);

        GameObject temp = new GameObject("temp");
        temp.transform.position = new Vector3(x, y, 0);

        prevTarget = temp;
        return temp.transform;
    }

    private void CreateOilSlick()
    {
        Instantiate(oilSlick, transform.position, Quaternion.identity);
        print("bloop");
    }
}
