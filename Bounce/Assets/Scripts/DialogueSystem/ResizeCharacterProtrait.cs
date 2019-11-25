using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueFunctionality;

public class ResizeCharacterProtrait : MonoBehaviour
{
	public ResizeBox dependentImage;
	private float changeSizeTime;
	private float changeSizeCounter;
	private float percentage;
	private Image image;
	private Vector2 imageSize;
	private bool completedResize = true;
	private bool increasing;
	private PBDialogueFunctionality dialogueFunctionality;

	// Start is called before the first frame update
	void Awake()
    {
		image = GetComponent<Image>();

		imageSize = image.rectTransform.sizeDelta;
	}

    // Update is called once per frame
    void Update()
    {
        if (dependentImage.CompletedResize || !increasing)
		{
			if (!completedResize && ((increasing && percentage < 1) || (!increasing && percentage > 0)))
			{
				changeSizeCounter += Time.deltaTime;

				percentage = increasing ? Mathf.Clamp01(changeSizeCounter / changeSizeTime) : 1 - Mathf.Clamp01(changeSizeCounter / changeSizeTime);

				image.rectTransform.sizeDelta = Vector2.up * (imageSize.x * percentage) + Vector2.right * imageSize.x;

				if (percentage == 1)
				{
					completedResize = true;
					dialogueFunctionality.FuncionalityCallback(true);
				}
				else if (percentage == 0)
				{
					completedResize = true;
					dialogueFunctionality.FuncionalityCallback(false);
					gameObject.SetActive(false);
				}
			}
		}
    }

	public void PrepareResize(PBDialogueFunctionality d, float pc, float s)
	{
		dialogueFunctionality = d;
		percentage = pc;
		changeSizeTime = s;
		changeSizeCounter = 0;

		increasing = (pc == 0);

		image.rectTransform.sizeDelta = Vector2.up * pc + Vector2.right * imageSize.x;

		completedResize = false;
	}
}
