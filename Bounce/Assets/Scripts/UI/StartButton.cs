using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    public float timeDelay;
    public byte sceneNumber;
    public void LoadNewScene()
    {
        Invoke("LoadNextScene", timeDelay);   
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
