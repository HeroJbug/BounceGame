using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Image pauseScreen;
    private static bool paused = false;
    private Image currentScreen;
    // Start is called before the first frame update
    void Start()
    {
        pauseScreen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
