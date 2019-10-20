using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void StartScript()
    {
        Invoke("GameOverScene", 2f);
    }
    public void GameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }
}
