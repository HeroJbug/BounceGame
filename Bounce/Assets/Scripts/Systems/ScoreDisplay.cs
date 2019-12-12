using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text ScoreBox;
    // Start is called before the first frame update
    void Start()
    {
        ScoreBox = GetComponent<Text>();
        ScoreBox.text = "Score: "+ScoreSystem.GetScore().ToString();
    }
}
