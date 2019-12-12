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

    BinaryFormatter bf;
    FileStream file;
    public static List<HighscoreItem> scores;
    readonly int newScore = ScoreSystem.GetScore();
    void Start()
    {
        bf = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/highScores.gd"))
        {
            //open file, load data
            file = File.Open(Application.persistentDataPath + "/highScores.gd",FileMode.Open);
            scores = (List<HighscoreItem>)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/highScores.gd");
            file.Close();
            scores = new List<HighscoreItem>(10);
        }
    }

    public void EnterName()
    {
        //make a new item, add it, sort the list by score, and then remove anything after the 10th score
        HighscoreItem newScoreItem = new HighscoreItem(newScore, nameBox.text);
        scores.Add(newScoreItem);
        scores.Sort();
        if(scores.Count >= 10)
            scores.RemoveRange(10, scores.Count - 10);
        SaveScores();
    }

    private void SaveScores()
    {
        file = File.Open(Application.persistentDataPath + "/highScores.gd", FileMode.Create);
        bf.Serialize(file, scores);
        file.Close();
        SceneManager.LoadScene(2);
    }
}
