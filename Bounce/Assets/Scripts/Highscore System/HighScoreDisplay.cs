using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplay : MonoBehaviour
{
    public Text highscoresField;
    // Start is called before the first frame update
    void Start()
    {
        List<HighscoreItem> scoreItems = HighScoreEntry.scores;
        for(int i = 0; i < scoreItems.Count; i++)
        {
            if(scoreItems[i] != null)
            {
                highscoresField.text += scoreItems[i].name + ": " + scoreItems[i].score + "\n";
            }
            else
            {
                highscoresField.text += "...........\n";
            }
        }
    }
}
