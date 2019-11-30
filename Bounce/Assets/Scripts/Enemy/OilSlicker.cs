using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlicker : Enemy
{
    public GameObject oilSlick;
    public float oilSlickSpawnTimer = 5f;
    private float oilSpawnTimerInternal;
    private GameObject prevTarget;
    private Queue<GameObject> last3Slicks;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        target = PickNewRandomTargetLocation();
        base.InitializeSelf();
        oilSpawnTimerInternal = oilSlickSpawnTimer;
        last3Slicks = new Queue<GameObject>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(oilSpawnTimerInternal <= 0 && !GetInKnockback())
        {
            CreateOilSlick();
            oilSpawnTimerInternal = oilSlickSpawnTimer;
            target = PickNewRandomTargetLocation();
            RequestNewPath();
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

        GameObject temp = new GameObject("OilSlickerTempTarget");
        temp.transform.position = new Vector3(x, y, 0);

        prevTarget = temp;
        return temp.transform;
    }

    private void CreateOilSlick()
    {
        anim.SetTrigger("DropOil");
        GameObject newSlick = Instantiate(oilSlick, transform.position, Quaternion.identity);
        last3Slicks.Enqueue(newSlick);
        RemoveExtraneousSlicks();
    }

    private void RemoveExtraneousSlicks()
    {
        if(last3Slicks.Count > 3)
        {
            GameObject toRemove = last3Slicks.Dequeue();
            Destroy(toRemove);
        }
    }
}
