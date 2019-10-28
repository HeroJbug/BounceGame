using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
	private PlayerCollision player;
	private Image bar;
	private float originalWidth;
	private float originalXpos;
	private float percentage;
	private float maxHp;

	// Start is called before the first frame update
	void Start()
    {
		player = FindObjectOfType<PlayerCollision>();
		bar = GetComponent<Image>();
		originalWidth = bar.rectTransform.rect.width;
		originalXpos = bar.transform.position.x;
		maxHp = player.hp;
    }

    // Update is called once per frame
    void Update()
    {
		percentage = player.hp / maxHp;

		float width = originalWidth * percentage;

		bar.transform.position = new Vector3(originalXpos - (originalWidth - width) / 2, bar.transform.position.y);

		bar.rectTransform.sizeDelta = new Vector2(width, bar.rectTransform.rect.height);
	}
}
