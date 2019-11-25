using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueFunctionality;

public class ResizeBox : MonoBehaviour
{
	[SerializeField]
	private float changeSizeTime;
	private float changeSizeCounter;
	[SerializeField]
	private float percentage;
	private Image image;
	[SerializeField]
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
		if (!completedResize && ((increasing && percentage < 1) || (!increasing && percentage > 0)))
		{
			changeSizeCounter += Time.deltaTime;

			percentage = increasing ? Mathf.Clamp01(changeSizeCounter / changeSizeTime) : 1 - Mathf.Clamp01(changeSizeCounter / changeSizeTime);

			image.rectTransform.sizeDelta = imageSize * percentage;

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

	public void StartResize(PBDialogueFunctionality d, float pc, float s)
	{
		dialogueFunctionality = d;
		percentage = pc;
		changeSizeTime = s;
		changeSizeCounter = 0;

		increasing = (pc == 0);

		image.rectTransform.sizeDelta = Vector2.one * pc;

		completedResize = false;
	}
}
