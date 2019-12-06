using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Hazard
{
	/// <summary>
	/// The damage to deal to entity
	/// </summary>
	public float damageToDeal;
    [SerializeField]
    protected float countdownTimer;
	[SerializeField]
	protected bool drawGizmos = true;

    private SpriteRenderer sr;

	// Start is called before the first frame update
	void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
		Initialize();
		type = HazardTypes.BOMB;
    }

    // Update is called once per frame
    void Update()
    {
        if (Landed)
		{
            countdownTimer -= Time.deltaTime;
            sr.color = new Color(1, countdownTimer / 3, countdownTimer / 3);
            if (countdownTimer <= 0)
            {
                foreach (string layerName in layersToAffect)
                {
                    if (ColInCircleAll(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit2D[] hits))
                    {
                        foreach (RaycastHit2D hit in hits)
                        {
                            //damage player by damageToDeal
                            if(hit.rigidbody.gameObject.GetComponent<PlayerCollision>() != null)
                            {
                                hit.rigidbody.gameObject.GetComponent<PlayerCollision>().TakeDamage(damageToDeal, false);
                            }
                            //esplode enemies
                            if(hit.rigidbody.gameObject.GetComponent<Enemy>() != null)
                            {
                                print("boom enemy");
								hit.rigidbody.gameObject.GetComponent<Enemy>().OnCollisionDeath();
							}
                        }
                    }
                    StartCoroutine(Explode());
                }
            }	
		}
		else
		{
			Falling();
		}
    }

    IEnumerator Explode()
    {
        sr.color = new Color(1, 1, 1);
        GetComponent<Animator>().SetTrigger("OnExplode");
        transform.localScale = new Vector3(3f, 3f, 1f);
        SoundSystem.system.PlaySFXMain("EnemyBoom1", 0.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

	public void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, targetRadius);
		}
	}
}
