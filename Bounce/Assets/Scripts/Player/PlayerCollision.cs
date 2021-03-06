﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
	public float enemyKnockBackForce = 25f;
	public float enemyDamage = 1f;
	public float hp = 3;
	[SerializeField]
	private float maxInvincibilityTime = 3;
	private float invinciblityTime;
	//public GameObject panel;
	public GameObject mainCamera;
	private SpriteRenderer mr;
	private Animator anim;
	Rigidbody2D rbody;
	private ScreenShake ss;
	private AudioSource source;
	private bool preppedMusic = false;
	private bool isInTutorialMode;

	[SerializeField]
	private float timeTillDeath = 1.5f;

	public delegate void PlayerDeathDelegate();
	public static event PlayerDeathDelegate ThisPlayerDeath;

	PlayerMovement moveRef;
	// Start is called before the first frame update
	void Start()
	{
		moveRef = this.GetComponent<PlayerMovement>();
		mr = GetComponent<SpriteRenderer>();
		rbody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		//mainCamera.transform.parent = transform;
		mainCamera = FindObjectOfType<Camera>().gameObject;
		ss = FindObjectOfType<ScreenShake>();
		source = GetComponent<AudioSource>();

		if ((isInTutorialMode = GetComponent<PlayerMovement>().isInTutorialMode))
		{
			enemyDamage = 0;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Vector2 moveDir = rbody.transform.position - collision.gameObject.GetComponent<Rigidbody2D>().transform.position;
			if (moveRef.PlayerIsBoosting())
			{
				collision.gameObject.GetComponent<Rigidbody2D>().AddForce(enemyKnockBackForce * -moveDir, ForceMode2D.Impulse);
				collision.gameObject.GetComponent<Enemy>().SetInKnockback(true);
			}
			else
			{
				//kinda inefficient but meh
				if (!collision.gameObject.GetComponent<TurtleEnemy>())
				{
					TakeDamage(enemyDamage, false);
					if (!isInTutorialMode)
					{
						collision.gameObject.GetComponent<Enemy>().OnCollisionDeath();
					}
				}
				else
				{
					if (!collision.gameObject.GetComponent<TurtleEnemy>().inHazardMode)
					{
						TakeDamage(enemyDamage, false);
						collision.gameObject.GetComponent<Enemy>().OnCollisionDeath();
					}
				}
			}
		}
	}

	private void Update()
	{
		if (hp <= 0)
		{
			if (!preppedMusic)
			{
				if (MultiplayerManager.manager.PlayerCount <= 1)
				{
					SoundSystem.system.StopMusic();
				}

				SoundSystem.system.StopSFXLooped(source);
				SoundSystem.system.PlaySFX(source, "PlayerDeath", 1);
				preppedMusic = true;
			}

			GetComponent<PlayerMovement>().dashIndicator.SetActive(false);
			StartCoroutine(OnDeath());
		}
		else if (invinciblityTime > 0)
		{
			mr.enabled = !mr.enabled;
			invinciblityTime -= Time.deltaTime;

			if (invinciblityTime <= 0)
			{
				mr.enabled = true;
			}
		}
	}

	IEnumerator OnDeath()
	{
		//is this clean? no. Does it work? yes.
		GetComponent<PlayerMovement>().SetBoostingFalse();
		rbody.velocity = Vector2.zero;
		anim.SetBool("OnDeath", true);
		yield return new WaitForSeconds(1.3f);
		ThisPlayerDeath();
		Invoke("ToDeath", timeTillDeath);
		//transform.DetachChildren();
	}

	private void ToDeath()
	{
		Destroy(this.gameObject);
	}

	public void TakeDamage(float amt, bool electric)
    {
		if (hp > 0)
		{
			if (invinciblityTime <= 0)
			{
				ss.StartShake();
				ScoreSystem.system.ResetScore();
				
				if (electric)
				{
					anim.SetTrigger("ElectricDamage");
					//SoundSystem.system.PlaySFX(source, "ShockSound", 1f);
				}
				SoundSystem.system.PlaySFX(source, "EnemyBoom2", 1f);
				hp -= amt;
				invinciblityTime = maxInvincibilityTime;
			}
		}
    }
}

