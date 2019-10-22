using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    public void LoadNewScene()
    {
        SceneManager.LoadScene(1);
    }
}
