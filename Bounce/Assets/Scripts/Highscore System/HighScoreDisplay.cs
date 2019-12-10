using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] entries = HighScoreEntry.Display;
        for(int i=0;i<10;i++)
        {
            transform.GetChild(i).GetComponent<Text>().text = entries[i];
        }
    }
}
