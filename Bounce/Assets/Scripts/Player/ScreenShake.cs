using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
	[SerializeField]
	private float shakeAmtMax = 5;
	[SerializeField]
	private float shakeTime = 0.5f;
	private float shakeAmt;
	private float time;
	private PlayerCollision player;

	// Start is called before the first frame update
	void Start()
    {
		player = GetComponentInParent<PlayerCollision>();
		this.transform.parent = player.transform;
	}

    // Update is called once per frame
    void Update()
    {
		if (time > 0)
		{
			time -= Time.deltaTime;

			time = time < 0 ? 0 : time;
		}

		shakeAmt = shakeAmtMax * (time / shakeTime);

		transform.localPosition = (Vector3)(Random.insideUnitCircle * shakeAmt) + Vector3.forward * transform.position.z;
	}

	public void StartShake()
	{
		time = shakeTime;
	}
}
