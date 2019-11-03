using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
	private PlayerCollision player;
	private Image bar;
	private float originalWidth;
	private float percentage;
	private float maxHp;

	// Start is called before the first frame update
	void Start()
    {
		player = FindObjectOfType<PlayerCollision>();
		bar = GetComponent<Image>();
		originalWidth = bar.rectTransform.rect.width;
		maxHp = player.hp;
    }

    // Update is called once per frame
    void Update()
    {
		percentage = player.hp / maxHp;

		bar.fillAmount = percentage;
	}
}
