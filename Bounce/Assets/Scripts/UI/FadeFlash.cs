using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFlash : MonoBehaviour
{
    public Text textToFade;
    public float fadeTime;
    private bool fade = false;

    private void OnEnable()
    {
        fade = true;
    }

    private void OnDisable()
    {
        fade = false;
        textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 1);
    }
    private void FixedUpdate()
    {
        if(fade)
        {
            float currAlpha = Mathf.Sin(Time.time / fadeTime);
            textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, currAlpha);
        }      
    }
}
