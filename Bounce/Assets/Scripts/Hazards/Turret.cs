using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Hazard, IFirable
{
	[SerializeField]
	protected Projectile projectilePrefab;
	[SerializeField]
	protected Vector2 bulletOffset;
	[SerializeField]
	protected int maxShots;
	[SerializeField]
	protected float shotCooldown;
	protected int shots;
	protected float cooldown;
	[SerializeField]
	protected GameObject turretBase;
	[SerializeField]
	protected bool drawGizmos = true;
    [SerializeField]
    protected List<Sprite> images;
    private SpriteRenderer sr;

	// Start is called before the first frame update
	void Awake()
    {
		Initialize();
		type = HazardTypes.TURRET;
		shots = maxShots;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Landed)
		{
            timer -= Time.deltaTime;
            if (timer <= 0)
                Destroy(this.gameObject);
            foreach (string layerName in layersToAffect)
			{
				if (cooldown <= 0 && shots > 0 && ColInCircle(transform.position, targetRadius, LayerMask.GetMask(layerName), out RaycastHit2D hit))
				{
					Vector3 firDir = hit.transform.position - this.transform.position;
					firDir.Normalize();

					float newZrot = Vector2.SignedAngle(Vector2.right, firDir);
                    ChooseCorrectSprite(newZrot);
					//transform.eulerAngles = Vector3.forward * newZrot;

					Projectile proj = Instantiate<Projectile>(projectilePrefab, transform.position + (Vector3)bulletOffset + Vector3.up * -1.57f, Quaternion.identity);
					proj.Direction = firDir;
					proj.Parent = this;

					cooldown = shots > 0 ? shotCooldown : 4 * shotCooldown;
					shots--;
				}
			}

			if (cooldown > 0)
			{
				cooldown -= Time.deltaTime;
			}
		}
		else
		{
			Falling();
		}

		turretBase.transform.eulerAngles = Vector3.zero;
	}

    //Is there a better way to do this? yes. Do I care since the semsster is almost over and this class isn't high on my list of importance? no.
    private void ChooseCorrectSprite(float angle)
    {
        angle += 180;;
        int choice = 0;
        if (157.5 < angle && angle <= 202.5)
        {
            choice = 0;
        }
        else if (202.5 < angle && angle <= 247.5)
        {
            choice = 1;
        }
        else if (247.5 < angle && angle <= 292.5)
        {
            choice = 2;
        }
        else if (292.5 < angle && angle <= 337.5)
        {
            choice = 3;
        }
        else if (337.5 < angle || (0 < angle && angle <= 22.5))
        {
            choice = 4;
        }
        else if(22.5 < angle && angle <= 67.5)
        {
            choice = 5;
        }
        else if(67.5 < angle && angle <= 112.5)
        {
            choice = 6;
        }
        else
        {
            choice = 7;
        }
        sr.sprite = images[choice];
    }

	public void OnDrawGizmos()
	{
		if (drawGizmos)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, targetRadius);
		}
	}

	public void BulletCallback()
	{
		shots++;
	}
}
