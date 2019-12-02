using System.Collections;
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

	PlayerMovement moveRef;
    // Start is called before the first frame update
    void Start()
    {
        moveRef = this.GetComponent<PlayerMovement>();
		mr = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mainCamera.transform.parent = transform;
		ss = GetComponentInChildren<ScreenShake>();
		source = GetComponent<AudioSource>();
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
                    collision.gameObject.GetComponent<Enemy>().OnCollisionDeath();
                }
                else
                {
                    if(!collision.gameObject.GetComponent<TurtleEnemy>().inHazardMode)
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
				SoundSystem.system.StopMusic();
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
        rbody.velocity = Vector2.zero;
        anim.SetBool("OnDeath", true);
        yield return new WaitForSeconds(1.3f);
        //transform.DetachChildren();
        Invoke("NextScene", 2f);
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
                    anim.SetTrigger("ElectricDamage");
				hp -= amt;
				invinciblityTime = maxInvincibilityTime;
			}
		}
    }

    public void NextScene()
    {
        SceneManager.LoadScene(5);
    }
}

