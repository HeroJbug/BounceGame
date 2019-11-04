using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
	[SerializeField]
	private float scoreMultiplier;
	[SerializeField]
	private float multiplierIcr;
	[SerializeField]
	private int score;
	[SerializeField]
	private float scoreDecreaseSpeed;
	[SerializeField]
	private int scoreIncreaseAmt;
	[SerializeField]
	private float multLimit = 9;

	public static ScoreSystem system;

	// Start is called before the first frame update
	void Start()
    {
        if (system == null)
		{
			system = this;
		}
		else if (system != this)
		{
			Destroy(this);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (scoreMultiplier > 0)
		{
			scoreMultiplier -= scoreDecreaseSpeed * Time.deltaTime;

			if (scoreMultiplier < 0)
			{
				scoreMultiplier = 0;
			}
		}

	}

	public void IncrementScore(int amt)
	{
		scoreMultiplier += Mathf.Clamp(scoreMultiplier + multiplierIcr, 0, multLimit - 1);
		int addToScore = Mathf.CeilToInt(amt * ScoreMultiplier);
		score += addToScore * scoreIncreaseAmt;
	}

	public void ResetScore()
	{
		scoreMultiplier = 0;
	}

	public float ScoreMultiplier
	{
		get
		{
			return scoreMultiplier;
		}
	}

	public int Score
	{
		get
		{
			return score;
		}
	}
}
