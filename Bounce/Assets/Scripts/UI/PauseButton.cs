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
            x.enabled = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                foreach(Image x in pauseScreen)
                    x.enabled = true;
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
        if(Input.GetMouseButtonDown(0))
        {
            if(paused)
            {
                float mX = Input.mousePosition.x, mY = Input.mousePosition.y;
                if (Mathf.Abs(mX - 573f) <= 75f && Mathf.Abs(mY - 183.5f) <= 15f)
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene(0);
                }
                if (Mathf.Abs(mX - 573f) <= 75f && Mathf.Abs(mY - 134.5f) <= 15f)
                {
                    Time.timeScale = 1f;
                    Application.Quit();
                }
                //print(Input.mousePosition);
                //foreach(Image x in pauseScreen)
                //{
                //    print(x.transform.position);
                //}
            }
        }
    }
}
