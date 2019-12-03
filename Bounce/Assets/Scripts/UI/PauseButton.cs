using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public List<Image> pauseScreen;
    private static bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Image x in pauseScreen)
            if(x != null)
                x.enabled = false;
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!paused)
            {
                foreach(Image x in pauseScreen)
                    x.enabled = true;
                print("Images enabled");
                Time.timeScale = 0f;
                paused = true;
            }
            else
            {
                foreach(Image x in pauseScreen)
                    x.enabled = false;
                Time.timeScale = 1f;
                paused = false;
            }
        }
    }
}
