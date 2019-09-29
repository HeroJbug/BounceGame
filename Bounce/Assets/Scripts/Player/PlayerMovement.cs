using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f, boostSpeed = 10f;
    Vector3 moveVec;
    Rigidbody rBody;
    public float boostTimer = 0.3f;
    float boostTimerCounter;
    bool isBoosting;
    // Start is called before the first frame update
    void Start()
    {
        moveVec = new Vector3();
        rBody = this.GetComponent<Rigidbody>();
        boostTimerCounter = boostTimer;
        isBoosting = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");
        //GetComponent<Rigidbody>().AddForce(speed*moveVec);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rBody.AddExplosionForce(boostSpeed, transform.position - moveVec, 5f, 0f, ForceMode.Impulse);
            isBoosting = true;
            Invoke("StopBoosting", boostTimer);
        }

        //if(isBoosting)
        //{
          //  boostTimerCounter -= Time.deltaTime;
            //if(boostTimerCounter <= 0)
            //{
              //  boostTimerCounter = boostTimer;
                //rBody.velocity = Vector3.zero;
                //isBoosting = false;
            //}
        //}
    }

    public void StopBoosting()
    {
        rBody.velocity = Vector3.zero;
        isBoosting = false;
    }

    public bool PlayerIsBoosting()
    {
        return isBoosting;
    }

    private void FixedUpdate()
    {
        rBody.MovePosition(rBody.position + moveVec * speed * Time.deltaTime);
    }
}
