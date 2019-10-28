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
    SpriteRenderer mr;
    public Sprite up, down, left, right;
    // Start is called before the first frame update
    void Start()
    {
        moveVec = new Vector3();
        rBody = this.GetComponent<Rigidbody2D>();
        boostTimerCounter = boostTimer;
        isBoosting = false;
        mr = GetComponent<SpriteRenderer>();
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
        else
        {

        }
        //Put this in the else block when we have boost animations
        if(Mathf.Abs(moveVec.x)>Mathf.Abs(moveVec.y))
        {
            mr.sprite = moveVec.x >=0?right:left;
        }
        else
        {
            mr.sprite = moveVec.y > 0 ? up : down;
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
