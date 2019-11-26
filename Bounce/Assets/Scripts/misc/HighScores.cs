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
    public GameObject ScoreLines;

    BinaryFormatter bf;
    FileStream file;
    SortedDictionary<int,LinkedList<string>> scores;
    //newScore is negative so while sorting, the highest scores come first
    readonly int newScore = -1*ScoreSystem.GetScore();
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
            scores = (SortedDictionary<int, LinkedList<string>>)bf.Deserialize(file);
            int numOfBetterScores = 0;
            foreach(KeyValuePair<int,LinkedList<string>> x in scores)
            {
                if (x.Key <= newScore)
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
            scores = new SortedDictionary<int, LinkedList<string>>();
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
        if (scores.ContainsKey(newScore))
            scores[newScore].AddFirst(nameBox.text);
        else
        {
            scores.Add(newScore, new LinkedList<string>());
            scores[newScore].AddFirst(nameBox.text);
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

        //Use for debugging
        //print(file.Name);

        foreach (KeyValuePair<int, LinkedList<string>> x in scores)
        {
            if (numOfBetterScores >= 10)
            {
                scores.Remove(x.Key);
                continue;
            }
            foreach (string s in x.Value)
            {
                if (numOfBetterScores >= 10)
                    break;

                //Do something with the values here for display purposes
                //print(x.Key + "," + s);
                string currentLine = s + ": " + (-1*x.Key);
                ScoreLines.transform.GetChild(numOfBetterScores).GetComponent<Text>().text = currentLine;
                //

                numOfBetterScores++;
            }
        }
        for (; numOfBetterScores < 10; numOfBetterScores++)
            ScoreLines.transform.GetChild(numOfBetterScores).GetComponent<Text>().text = "..........";
        bf.Serialize(file, scores);
        file.Close();
    }
}
