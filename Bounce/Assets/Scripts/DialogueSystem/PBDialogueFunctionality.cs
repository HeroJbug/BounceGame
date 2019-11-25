using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueFunctionality;

public class PBDialogueFunctionality : IDialogueFunctionality
{
	public Color normalButtonColor;
	public Color highlightedButtonColor;
	public Color pressedButtonColor;
	public Sprite SelectedImage;
	public Sprite DeselectedImage;
	private Vector2 dialogueBoxPos;
	public float DialogueBoxChangeSizeTime;
	private int callbacks = 0;

	public void MainOnDeselectChoiceBox()
	{
		DialogueManager.manager.SelectedChoiceBox.ChangeColor(Color.white, Color.white, Color.white);
	}

	public void MainOnSelectChoiceBox()
	{
		DialogueManager.manager.SelectedChoiceBox.ChangeColor(normalButtonColor, highlightedButtonColor, pressedButtonColor);
	}

	public void MainPositionButtons(DialoguePacket packet, GameObject[] buttons, int idx)
	{
		float xOffset = dialogueBoxPos.x;

		if (buttons.Length % 2 == 0)
		{
			xOffset = (xOffset - 100) - (200 * ((buttons.Length / 2) - 1));
		}
		else
		{
			xOffset -= 200 * (buttons.Length / 2);
		}

		buttons[idx].transform.position = new Vector3(xOffset + (200 * idx), packet.images[0].transform.position.y - (packet.images[0].rectTransform.rect.height / 2) * DialogueManager.manager.dialogueCanvas.GetComponent<Canvas>().scaleFactor, DialogueManager.manager.dialogueCanvas.transform.position.z);
	}

	public void MainUpdateImages(DialoguePacket packet)
	{
		//nothing
	}

	public void MainUpdateText(DialoguePacket packet)
	{
		packet.textObjects[1].text = DialogueManager.manager.speakers[packet.dialogueItems[packet.index].speakerID];
		string dialogue = packet.dialogueItems[packet.index].text;
		dialogue = DialogueManager.manager.PlaceKeywords(dialogue);
		packet.textObjects[0].text = dialogue;
	}

	private void PBSetActive(DialoguePacket packet, bool status)
	{
		foreach (Image image in packet.images)
		{
			image.gameObject.SetActive(true);
			image.rectTransform.sizeDelta = status ? Vector2.zero : image.rectTransform.sizeDelta;
			image.gameObject.GetComponent<ResizeBox>().StartResize(this, status ? 0 : 1, DialogueBoxChangeSizeTime);
			callbacks++;
		}

		foreach (Text textObj in packet.textObjects)
		{
			textObj.gameObject.SetActive(status);
			textObj.text = "";
		}
	}

	public void FuncionalityCallback(bool status)
	{
		callbacks--;

		if (callbacks == 0)
		{
			DialogueManager.manager.DialogueBoxActive = status;
		}
	}

	public void Start()
	{
		dialogueBoxPos = DialogueManager.manager.dialogueCanvas.transform.position;

		DialogueManager.manager.SetActive = PBSetActive;
	}
}
