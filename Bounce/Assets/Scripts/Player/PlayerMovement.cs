using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public int joyPadIndex;

	public float speed = 5f, boostSpeed = 40f, initialBoostSpeed = 80f;
	public float boostAcceleration = 40f;
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
	[HideInInspector]
	public Vector2 boostDir;
    private Camera cam;
    public GameObject dashIndicator;
	[SerializeField]
	private float deathAnimTime;
	private float deathAnimCounter = 0;
	[HideInInspector]
	public Vector2 slipVec;
	public float frictionalAcceleration = 0.25f;
	[HideInInspector]
	public float slipSpeed;
	private float playIdleSoundTimeDuration;
	Vector2 boostVec;
	Vector2 aim;
	public bool isInTutorialMode = false;
	private AudioSource source;


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
		aim = Vector2.up;
		source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInTutorialMode || !DialogueManager.manager.DialogueBoxActive)
		{
			//if we're dead ignore the rest of the work
			if (mainAnim.GetBool("OnDeath"))
			{
				if (deathAnimCounter >= deathAnimTime)
				{
					GetComponent<SpriteRenderer>().enabled = false;
				}
				else
				{
					deathAnimCounter += Time.deltaTime;
				}

				return;
			}
			moveVec.x = Input.GetAxisRaw("Horizontal");
			moveVec.y = Input.GetAxisRaw("Vertical");
			int currentDir = GetDirThisFrame();
			mainAnim.SetInteger("Direction", currentDir);
			if (Input.GetButtonDown("Dash") && boostCooldownCounter <= 0)
			{
				Boost();
			}

			if (isBoosting)
			{
				boostTimerCounter -= Time.deltaTime;
				boostCooldownCounter = boostCooldown * (1 - (boostTimerCounter / boostTimer));

				if (boostTimerCounter <= 0)
				{
					rBody.velocity = Vector3.zero;
					isBoosting = false;
					boostCooldownCounter = boostCooldown;
				}
				mainAnim.SetBool("isBoosting", true);
			}
			else
			{
				if (mainAnim.GetBool("isBoosting"))
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
    }

    private void UpdateAimPos()
    {
		if (Input.GetJoystickNames().Length == 0)
		{
			//calculate aim based on mouse
			Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
			aim = mousePos - (Vector2)transform.position;
		}
		else 
		{
			float joypadX = Input.GetAxis("Aim_Horizontal");
			float joypadY = Input.GetAxis("Aim_Vertical");

			aim = new Vector2(joypadX == 0 ? aim.x : joypadX, joypadY == 0 ? aim.y : joypadY);
		}

		aim.Normalize();

		dashIndicator.transform.localPosition = aim * 25;
    }

    private void Boost()
    {
        boostVec = dashIndicator.transform.position - transform.position;
        boostVec.Normalize();
        ChooseCorrectBoostAnim(boostVec);
		//rBody.AddForce((boostSpeed * boostVec), ForceMode2D.Impulse);
		boostDir = aim;
		boostSpeed = initialBoostSpeed;
		SoundSystem.system.StopSFXLooped(source);
		SoundSystem.system.PlaySFX(source, "DashSound", 1);
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
		if (!isInTutorialMode || !DialogueManager.manager.DialogueBoxActive)
		{
			UpdateAimPos();

			if (!mainAnim.GetBool("isBoosting") && !mainAnim.GetBool("OnDeath"))
			{
				rBody.MovePosition(rBody.position + slipVec * slipSpeed * Time.deltaTime + (Vector2)moveVec * speed * Time.deltaTime);

				if (!source.isPlaying)
				{
					SoundSystem.system.PlaySFXLooped(source, "JetpackIdle");
				}
			}
			else if (mainAnim.GetBool("isBoosting"))
			{
				//rBody.AddForce(slipVec * slipSpeed);
				rBody.MovePosition(rBody.position + slipVec * slipSpeed * Time.deltaTime + (Vector2)boostDir * boostSpeed * Time.deltaTime);

				boostSpeed += boostAcceleration;
			}

			slipSpeed = Mathf.Clamp(slipSpeed - frictionalAcceleration * Time.deltaTime, 0, slipSpeed);
		}
		else
		{
			SoundSystem.system.StopSFXLooped(source);
		}
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

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawLine(transform.position, transform.position + (Vector3)aim * 25);
	}
}
