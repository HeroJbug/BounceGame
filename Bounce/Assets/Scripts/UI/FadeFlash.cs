using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFlash : MonoBehaviour
{
    public Text textToFade;
    public float fadeTime;
	private float timer;
	private float multiplier;
	private bool fade = false;

	private void OnEnable()
    {
		timer = 0;
		multiplier = 1;
		textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 0);
		fade = true;
    }

    private void OnDisable()
    {
        fade = false;
        textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 1);
    }
    private void Update()
    {
        if(fade)
        {
			if (textToFade.color.a <= 0)
			{
				SoundSystem.system.PlaySFXMain("SirenSound", 0.5f);
			}

			timer = Mathf.Clamp(timer + multiplier * Time.deltaTime, 0, fadeTime);

			float currAlpha = timer / fadeTime;
			textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, currAlpha);

			if (currAlpha == 1 || currAlpha == 0) { multiplier *= -1; }
		}

		/*if (timer > 0)
		{
			timer -= Time.deltaTime;

			if (timer <= 0)
				gameObject.SetActive(false);
		}*/
    }
}
