using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    public float enemyKnockBackForce = 25f;
    public float enemyDamage = 1f;
	public float hp = 3;
	public float maxInvincibilitTime = 3;
	public float invinciblityTime;
	public GameObject panel;
	private MeshRenderer mr;

    PlayerMovement moveRef;
    // Start is called before the first frame update
    void Start()
    {
        moveRef = this.GetComponent<PlayerMovement>();
		mr = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ContactPoint hitPoint = collision.GetContact(0);
            if (moveRef.PlayerIsBoosting())
                collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(enemyKnockBackForce, transform.position - hitPoint.normal, 5f, 0f, ForceMode.Impulse);
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
			panel.SetActive(true);
			this.gameObject.SetActive(false);
			Time.timeScale = 0;
		}

		if (invinciblityTime > 0)
		{
			mr.enabled = !mr.enabled;
			invinciblityTime -= Time.deltaTime;

			if (invinciblityTime <= 0)
			{
				mr.enabled = true;
			}
		}
	}

	public void TakeDamage(float amt)
    {
		if (invinciblityTime <= 0)
		{
			hp -= amt;
			invinciblityTime = maxInvincibilitTime;
		}
		
    }
}

