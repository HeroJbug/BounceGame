using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f, boostSpeed = 40f;
	[SerializeField]
    Vector3 moveVec;
    Rigidbody2D rBody;
    public float boostTimer = 0.4f;
    float boostTimerCounter;
    bool isBoosting;
	public float boostCooldown;
	private float boostCooldownCounter;
    SpriteRenderer mr;
    private Animator mainAnim;
    private int dir;
    private Camera cam;
    public GameObject dashIndicator;
	public Vector2 slipVec;
	public float frictionalAcceleration = 0.25f;
	public float slipSpeed;
	Vector2 boostVec;
	// Start is called before the first frame update
	void Start()
    {
        mainAnim = GetComponent<Animator>();
        moveVec = new Vector3();
        rBody = this.GetComponent<Rigidbody2D>();
        boostTimerCounter = boostTimer;
        isBoosting = false;
        mr = GetComponent<SpriteRenderer>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //if we're dead ignore the rest of the work
        if(mainAnim.GetBool("OnDeath"))
        {
            return;
        }
        moveVec.x = Input.GetAxisRaw("Horizontal");
        moveVec.y = Input.GetAxisRaw("Vertical");
        int currentDir = GetDirThisFrame();
        mainAnim.SetInteger("Direction", currentDir);
        UpdateAimPos();
        if (Input.GetButtonDown("Dash") && boostCooldownCounter <= 0)
        {
            Boost();
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
            mainAnim.SetBool("isBoosting", true);
        }
        else
        {
            if(mainAnim.GetBool("isBoosting"))
                mainAnim.SetBool("isBoosting", false);
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

    private void UpdateAimPos()
    {
        //calculate based on mouse first then controller so that controller overrides if necessary
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aim = mousePos - (Vector2)transform.position;
        Vector2 lookDir = mousePos - rBody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        if(Mathf.Abs(Input.GetAxisRaw("Aim_Horizontal")) > 0.1 || Mathf.Abs(Input.GetAxisRaw("Aim_Vertical")) > 0.1)
            aim = new Vector2(Input.GetAxis("Aim_Horizontal"), Input.GetAxis("Aim_Vertical"));
        if(aim.magnitude > 0.0f)
        {
            aim.Normalize();
            aim *= 25f;
            dashIndicator.transform.localPosition = Vector2.Lerp(dashIndicator.transform.localPosition, aim, 0.1f);
        }
    }

    private void Boost()
    {
        boostVec = dashIndicator.transform.position - transform.position;
        boostVec.Normalize();
        ChooseCorrectBoostAnim(boostVec);
        rBody.AddForce((boostSpeed * boostVec) + (slipVec * slipSpeed), ForceMode2D.Impulse);
        isBoosting = true;
        boostTimerCounter = boostTimer;
    }

    private void ChooseCorrectBoostAnim(Vector2 dir)
    {
        if (dir.y >= 0.7)
        {
            //up
            mainAnim.SetInteger("BoostDirection", 0);
        }
        else if(dir.x >= 0.7)
        {
            //right
            mainAnim.SetInteger("BoostDirection", 1);
        }
        else if(dir.y <= -0.7)
        {
            //down
            mainAnim.SetInteger("BoostDirection", 2);
        }
        else if(dir.x <= -0.7)
        {
            //left
            mainAnim.SetInteger("BoostDirection", 3);
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
        if(!mainAnim.GetBool("isBoosting") && !mainAnim.GetBool("OnDeath"))
		{
			rBody.MovePosition(rBody.position + slipVec * slipSpeed * Time.deltaTime + new Vector2(moveVec.x, moveVec.y) * speed * Time.deltaTime);
		}

		slipSpeed = Mathf.Clamp(slipSpeed - frictionalAcceleration * Time.deltaTime, 0, slipSpeed);
	}

	public float BoostCooldownCounter
	{
		get
		{
			return boostCooldownCounter;
		}
	}

	public Vector3 MoveVector
	{
		get
		{
			return moveVec;
		}
	}
}
