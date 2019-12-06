using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreEntry : MonoBehaviour
{
    public InputField nameBox;
    public Text newScoreDisplay;
    public static string[] Display = new string[10];

    BinaryFormatter bf;
    FileStream file;
    SortedDictionary<int,LinkedList<string>> scores;
    //newScore is negative so while sorting, the highest scores come first
    readonly int newScore = -1*ScoreSystem.GetScore();
    // Start is called before the first frame update
    void Start()
    {
        bf = new BinaryFormatter();
        //newScoreDisplay.text = "Score: " + (-1 * newScore);

		//Debug.Log("path: " + Application.persistentDataPath);

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
            file.Close();
            scores = new SortedDictionary<int, LinkedList<string>>();
            EnterScore();
        }
    }

    private void EnterScore()
    {
        file.Close();
    }

    public void EnterName()
    {
        if (scores.Count != 0 && scores.ContainsKey(newScore))
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
        int numOfBetterScores = 0;

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

                //Do something with the values here for Display purposes
                //print(x.Key + "," + s);
                string currentLine = s + ": " + (-1*x.Key);
                Display[numOfBetterScores] = currentLine;
                //

                numOfBetterScores++;
            }
        }
        for (; numOfBetterScores < 10; numOfBetterScores++)
            Display[numOfBetterScores] = "..........";
        file = File.Open(Application.persistentDataPath + "/highScores.gd", FileMode.Create);
        //Use for debugging
        //print(file.Name);
        bf.Serialize(file, scores);
        file.Close();
        SceneManager.LoadScene(2);
    }
}
