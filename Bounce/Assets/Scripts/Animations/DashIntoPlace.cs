using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIntoPlace : MonoBehaviour
{
    public float startDelay,duration;
    public GameObject startPoint, endPoint;
    Vector3 startPos, endPos;
    bool finished = true;
    float timePassed = 0f;

    private void Start()
    {
        startPos = startPoint.transform.position;
        endPos = endPoint.transform.position;
        transform.position = startPos;
        Invoke("StartTimer", startDelay);
    }

    public void StartTimer()
    {
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!finished)
            timePassed += Time.deltaTime;
        if (timePassed >= duration)
        {
            finished = true;
            transform.position = endPos;
        }
        if (!finished)
            transform.position = (startPos - endPos) * ((timePassed - duration) * (timePassed - duration) / (duration * duration)) + endPos;
    }
}
