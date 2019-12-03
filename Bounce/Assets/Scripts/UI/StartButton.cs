using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    public float timeDelay;
	public float musicFadeOutTime;
    public byte sceneNumber;
    public void LoadNewScene()
    {
		SoundSystem.system.StopMusicFadeOut(musicFadeOutTime);

		Invoke("LoadNextScene", timeDelay);   
    }

    private void LoadNextScene()
    {
        if(sceneNumber<255)
            SceneManager.LoadScene(sceneNumber);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
