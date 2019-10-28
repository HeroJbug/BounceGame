using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
	private PlayerMovement player;
	private Image image;

	// Start is called before the first frame update
	void Start()
    {
		player = FindObjectOfType<PlayerMovement>();
		image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
		float percentage = 1 - (player.BoostCooldownCounter / player.boostCooldown);

		image.fillAmount = percentage;
	}
}
