using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Image pauseScreen;
    private static bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseScreen.enabled = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print(transform.gameObject.name);
            if (!paused)
            {
                pauseScreen.enabled = true;
                Time.timeScale = 0f;
                paused = true;
            }
            else
            {
                pauseScreen.enabled = false;
                Time.timeScale = 1f;
                paused = false;
            }
        }
    }
}
