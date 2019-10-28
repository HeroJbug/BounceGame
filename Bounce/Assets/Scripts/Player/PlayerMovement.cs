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
	public float boostCooldown;
	private float boostCooldownCounter;
    SpriteRenderer mr;
    private Animator mainAnim;
    private int dir;
    // Start is called before the first frame update
    void Start()
    {
        mainAnim = GetComponent<Animator>();
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
        int currentDir = GetDirThisFrame();
        mainAnim.SetInteger("Direction", currentDir);
        if (Input.GetKeyDown(KeyCode.Space) && boostCooldownCounter <= 0)
        {
            rBody.AddForce(boostSpeed * moveVec, ForceMode2D.Impulse);
            isBoosting = true;
			boostTimerCounter = boostTimer;
		}

        if(isBoosting)
        {
            boostTimerCounter -= Time.deltaTime;
			boostCooldownCounter = boostCooldown * (1 - (boostTimerCounter / boostTimer));

            if(boostTimerCounter <= 0)
            {
                rBody.velocity = Vector3.zero;
                isBoosting = false;
				boostCooldownCounter = boostCooldown;
			}
            mainAnim.SetBool("isDashing", true);
        }
        else
        {
            if(mainAnim.GetBool("isDashing"))
                mainAnim.SetBool("isDashing", false);
        }

		if (!isBoosting && boostCooldownCounter > 0)
		{
			boostCooldownCounter -= Time.deltaTime;
			if (boostCooldownCounter <= 0)
			{
				boostCooldownCounter = 0;
			}
		}
    }

    private int GetDirThisFrame()
    {
        int returnVal = 0;
        if (Mathf.Abs(moveVec.x) > Mathf.Abs(moveVec.y))
        {
            returnVal = moveVec.x >= 0 ? 1 : 3;
        }
        else
        {
            returnVal = moveVec.y > 0 ? 0 : 2;
        }
        return returnVal;
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

	public float BoostCooldownCounter
	{
		get
		{
			return boostCooldownCounter;
		}
	}
}
