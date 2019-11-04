using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
	[SerializeField]
	private float shakeDecreaseSpeed = 0.35f;
	[SerializeField]
	private float shakeAmtMax = 5;
	private float shakeAmt;
	private PlayerCollision player;
	[SerializeField]
	private float ShakeSpeed = 2.5f;
	private Vector3 shakePos;

	// Start is called before the first frame update
	void Start()
    {
		player = GetComponentInParent<PlayerCollision>();
		this.transform.parent = player.transform;
	}

    // Update is called once per frame
    void Update()
    {
		if (player.hp > 0)
		{
			shakePos = Vector3.zero + Vector3.forward * transform.position.z;
			if (shakeAmt > 0)
			{
				shakePos += new Vector3(Mathf.Lerp(-shakeAmt, shakeAmt, Mathf.Round(Random.value)), Mathf.Lerp(-shakeAmt, shakeAmt, Mathf.Round(Random.value)));

				shakeAmt -= shakeDecreaseSpeed * Time.deltaTime;

			}
			else
			{
				shakePos = Vector3.zero + Vector3.forward * transform.position.z;
			}

			transform.localPosition = shakePos;
		}
    }

	public void StartShake()
	{
		shakeAmt = shakeAmtMax;
	}
}
