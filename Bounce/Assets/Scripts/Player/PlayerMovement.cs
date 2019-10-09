using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f, boostSpeed = 40f;
    Vector3 moveVec;
    Rigidbody2D rBody;
    public float boostTimer = 0.3f;
    float boostTimerCounter;
    bool isBoosting;
    // Start is called before the first frame update
    void Start()
    {
        moveVec = new Vector3();
        rBody = this.GetComponent<Rigidbody2D>();
        boostTimerCounter = boostTimer;
        isBoosting = false;
    }

    // Update is called once per frame
    void Update()
    {
        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rBody.AddForce(boostSpeed * moveVec, ForceMode2D.Impulse);
            isBoosting = true;
        }

        if(isBoosting)
        {
            boostTimerCounter -= Time.deltaTime;
            if(boostTimerCounter <= 0)
            {
                boostTimerCounter = boostTimer;
                rBody.velocity = Vector3.zero;
                isBoosting = false;
            }
        }
    }

    public bool PlayerIsBoosting()
    {
        return isBoosting;
    }

    private void FixedUpdate()
    {
        if(!isBoosting)
            rBody.MovePosition(rBody.position + new Vector2(moveVec.x, moveVec.y) * speed * Time.deltaTime);
    }
}
