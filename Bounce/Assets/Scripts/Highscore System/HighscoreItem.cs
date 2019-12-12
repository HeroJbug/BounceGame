using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighscoreItem : System.IComparable<HighscoreItem>
{
    public int score;
    public string name;

    public HighscoreItem(int _score, string _name)
    {
        score = _score;
        name = _name;
    }

    public int CompareTo(HighscoreItem other)
    {
        if(score > other.score)
            return -1;
        if(score <  other.score)
            return 1;
        return 0;
    }
}
