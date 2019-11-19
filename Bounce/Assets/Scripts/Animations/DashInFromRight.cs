using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashInFromRight : MonoBehaviour
{
    public float duration,startX,endX;
    bool finished = true;
    float timePassed = 0f;

    private void Start()
    {
        transform.position = new Vector3(startX, transform.position.y, transform.position.z);
        Invoke("StartTimer", 0.5f);
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
            transform.position = new Vector3(endX, transform.position.y, transform.position.z);
        }
        if(!finished)
            transform.position = new Vector3(endX + ((startX-endX)*(timePassed-duration)*(timePassed-duration))/(duration*duration), transform.position.y, transform.position.z);
    }
}
