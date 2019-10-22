using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    public byte sceneNumber;
    public void LoadNewScene()
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
