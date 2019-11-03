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
	public float maxInvincibilitTime = 3;
	public float invinciblityTime;
    //public GameObject panel;
    public GameObject mainCamera;
	private SpriteRenderer mr;
    private Animator anim;
    Rigidbody2D rbody;

    PlayerMovement moveRef;
    // Start is called before the first frame update
    void Start()
    {
        moveRef = this.GetComponent<PlayerMovement>();
		mr = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mainCamera.transform.parent = transform;
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
                TakeDamage(enemyDamage);
                Destroy(collision.gameObject);
            }
        }
    }

	private void Update()
	{
		if (hp <= 0)
		{
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
        rbody.velocity = Vector2.zero;
        anim.SetBool("OnDeath", true);
        yield return new WaitForSeconds(1.3f);
        transform.DetachChildren();
        Invoke("NextScene", 2f);
    }

	public void TakeDamage(float amt)
    {
		if (invinciblityTime <= 0)
		{
			hp -= amt;
			invinciblityTime = maxInvincibilitTime;
		}
		
    }

    public void NextScene()
    {
        SceneManager.LoadScene(2);
    }
}

