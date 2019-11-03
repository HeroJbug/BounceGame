using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
	[SerializeField]
	private int currentScoreCounter;
	private Image counterFG;
	private Image counterBG;
	
	// Start is called before the first frame update
    void Start()
    {
		Image[] images = GetComponentsInChildren<Image>();
		counterFG = images[1];
		counterBG = images[0];
    }

    // Update is called once per frame
    void Update()
    {
		if (ScoreSystem.system.ScoreMultiplier > 0)
		{
			counterFG.enabled = true;
			counterBG.enabled = true;
			currentScoreCounter = (int)ScoreSystem.system.ScoreMultiplier;

			float percentage = ScoreSystem.system.ScoreMultiplier % 1;

			counterFG.fillAmount = percentage;
		}
		else
		{
			counterFG.enabled = false;
			counterBG.enabled = false;
		}
    }
}
