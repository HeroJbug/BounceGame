using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public float waitTime,fadeSpeed;
    public void Initiate()
    {
        Invoke("StartFading", waitTime);   
    }

    private void StartFading()
    {
        GetComponent<Image>().CrossFadeAlpha(1.0f, fadeSpeed, true);
    }
}
