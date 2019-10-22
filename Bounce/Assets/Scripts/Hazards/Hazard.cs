
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HazardTypes : int
{
	SPIKES,
	TURRET,
	OILSPILL,
	BOMB,
    BARRIER
}

public abstract class Hazard : MonoBehaviour
{
	[SerializeField]
	protected float startingDistance = 100;
	[SerializeField]
    protected float targetRadius = 1.03f;
	[SerializeField]
	protected float fallSpeed = 100;
    [SerializeField]
    protected List<string> layersToAffect;
    private bool hasLanded = false;
	private float height;
	protected HazardTypes type;
	[SerializeField]
	private GameObject shadowObj;
	private GameObject shadow;
	private Vector3 pos;
	private float shadowAlpha;

	protected void Initialize()
	{
		height = startingDistance;
		pos = transform.position;
		shadow = Instantiate(shadowObj, new Vector3(0, 0, this.transform.position.z - 0.3f), transform.rotation, transform);
		shadow.transform.position -= Vector3.up * (height + 0.6f);
		//shadow.transform.localScale = new Vector3(1.7f, 0.8f);
		var color = shadow.GetComponent<SpriteRenderer>().color;
		shadowAlpha = color.a;
		color.a = 0;
		shadow.GetComponent<SpriteRenderer>().color = color;
		transform.position += Vector3.up * height; 
	}

	protected bool ColInCircleAll(Vector3 origin, float radius, int layer, out RaycastHit2D[] hits)
    {
		hits = Physics2D.CircleCastAll(origin, radius, Vector2.right, 0, layer);
		return (hits.Length > 0);
    }

	protected bool ColInCircle(Vector3 origin, float radius, int layer, out RaycastHit2D hit)
	{
		hit = new RaycastHit2D();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, Vector2.right, 0, layer);
		//RaycastHit[] hits = Physics.SphereCastAll(origin, radius, Vector3.forward, 0, layer);
		if (hits != null && hits.Length > 0)
		{
			hit = hits[0];
			return true;
		}
		return false;
	}

	protected void Falling()
	{
		height -= (fallSpeed * Time.deltaTime);

		transform.position = pos + Vector3.up * height;
		shadow.transform.position = this.transform.position - Vector3.up * (height + 0.6f);

		var color = shadow.GetComponent<SpriteRenderer>().color;
		color.a = shadowAlpha * (1 - height/startingDistance);
		shadow.GetComponent<SpriteRenderer>().color = color;

		hasLanded = (height <= 0);
		if (hasLanded)
		{
			transform.position = pos;
			shadow.transform.position = new Vector3 (pos.x, pos.y - 0.6f, pos.z - 0.3f);
		}
	}

	public bool Landed
	{
		get
		{
			return hasLanded;
		}
	}

	public HazardTypes Type
	{
		get
		{
			return type;
		}
	}
}
