using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour
{
    public List<GameObject> highScoreObjects, noHighScoreObjects;
    public InputField nameBox;
    public byte scoreNumber = 10;

    BinaryFormatter bf;
    FileStream file;
    SortedDictionary<int,List<string>> scores;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject x in highScoreObjects)
            x.SetActive(false);
        foreach (GameObject x in noHighScoreObjects)
            x.SetActive(false);
        bf = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/highScores.gd"))
        {
            file = File.Open(Application.persistentDataPath + "/highScores.gd",FileMode.Open);
            scores = (SortedDictionary<int, List<string>>)bf.Deserialize(file);
            int numOfBetterScores = 0;
            foreach(KeyValuePair<int,List<string>> x in scores)
            {
                if (x.Key <= ScoreSystem.GetScore())
                    break;
                numOfBetterScores+=x.Value.Count;
                if (numOfBetterScores >= 10)
                    break;
            }
            if (numOfBetterScores >= 10)
                SaveScores();
            else
                EnterScore();
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/highScores.gd");
            scores = new SortedDictionary<int, List<string>>();
            EnterScore();
        }
    }

    private void EnterScore()
    {
        foreach (GameObject x in highScoreObjects)
            x.SetActive(true);
    }

    public void EnterName()
    {
        if (scores.ContainsKey(ScoreSystem.GetScore()));
            scores[ScoreSystem.GetScore()].Add(nameBox.text);
        else
        {
            scores.Add(ScoreSystem.GetScore(), nameBox.text);
        }
        SaveScores();
    }

    private void SaveScores()
    {
        foreach (GameObject x in highScoreObjects)
            x.SetActive(false);
        foreach (GameObject x in noHighScoreObjects)
            x.SetActive(true);
        int numOfBetterScores = 0;
        foreach (KeyValuePair<int, List<string>> x in scores)
        {
            print(x.Key+","+x.Value);
            if (numOfBetterScores >= 10)
            {
                scores.Remove(x.Key);
                continue;
            }
            numOfBetterScores++;
        }
        bf.Serialize(file, scores);
        file.Close();
    }
}
