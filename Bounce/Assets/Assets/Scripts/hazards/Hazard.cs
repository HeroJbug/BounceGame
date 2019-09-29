
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HazardTypes : int
{
	SPIKES,
	TURRENT,
	OILSPILL,
	BOMB
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
		shadow = Instantiate(shadowObj, new Vector3(0, 0, this.transform.position.z + 0.3f), transform.rotation, transform);
		shadow.transform.position -= Vector3.up * (height + 0.6f);
		shadow.transform.localScale = new Vector3(0.17f, 0.08f);
		var color = shadow.GetComponent<SpriteRenderer>().color;
		shadowAlpha = color.a;
		color.a = 0;
		shadow.GetComponent<SpriteRenderer>().color = color;
		transform.position += Vector3.up * height; 
	}

	protected bool ColInCircleAll(Vector2 origin, float radius, int layer, out RaycastHit2D[] hits)
    {
        hits = Physics2D.CircleCastAll(origin, radius, Vector2.zero, 1, layer);
        return (hits.Length > 0);
    }

	protected bool ColInCircle(Vector2 origin, float radius, int layer, out RaycastHit2D hit)
	{
		hit = Physics2D.CircleCast(origin, radius, Vector2.zero, 1, layer);
		return hit;
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
			shadow.transform.position = new Vector3 (pos.x, pos.y - 0.6f, pos.z + 0.3f);
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
