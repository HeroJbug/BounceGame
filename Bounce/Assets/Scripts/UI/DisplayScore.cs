using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
	private Text scoreDisplayer;
	
	// Start is called before the first frame update
    void Start()
    {
		scoreDisplayer = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
		scoreDisplayer.text = "Score: " + ScoreSystem.system.Score;
    }
}
