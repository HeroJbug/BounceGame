using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
	private PlayerMovement[] players;
    private PlayerMovement player;
	private Image image;
    public int playerNum;

	// Start is called before the first frame update
	void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
		if (player != null)
		{
			float percentage = 1 - (player.BoostCooldownCounter / player.boostCooldown);

			image.fillAmount = percentage;
		}
		else
		{
			players = FindObjectsOfType<PlayerMovement>();
			foreach (PlayerMovement p in players)
			{
				if (p.playerNum == playerNum)
				{
					player = p;
				}
			}
			image = GetComponent<Image>();
		}
	}
}
