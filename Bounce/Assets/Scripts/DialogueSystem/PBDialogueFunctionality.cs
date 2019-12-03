using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogueFunctionality;

public class PBDialogueFunctionality : MonoBehaviour, IDialogueFunctionality
{
	public Color normalButtonColor;
	public Color highlightedButtonColor;
	public Color pressedButtonColor;
	public float BoxChangeSizeTime;
	[SerializeField]
	private float buttonPadding;
	private int callbacks = 0;
	[SerializeField]
	private bool autoProgress;
	[SerializeField]
	private bool ShowDialogueOnce;
	[SerializeField]
	private Sprite[] charProtraits;

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
		float x = packet.images[0].transform.localPosition.x + (packet.images[0].rectTransform.rect.width / 2) - (buttons[idx].GetComponent<Image>().rectTransform.rect.width / 2) - buttonPadding;
		float y = packet.images[0].transform.localPosition.y - (packet.images[0].rectTransform.rect.height / 2) + (buttons[idx].GetComponent<Image>().rectTransform.rect.height / 2) + buttonPadding;
		
		buttons[idx].transform.localPosition = new Vector2(x, y);
	}

	public void MainUpdateImages(DialoguePacket packet)
	{
		int currentProtraitIndex = packet.dialogueItems[packet.index].charPortraitID;

		packet.images[1].sprite = charProtraits[currentProtraitIndex];
	}

	public void MainUpdateText(DialoguePacket packet)
	{
		packet.textObjects[1].text = DialogueManager.manager.speakers[packet.dialogueItems[packet.index].speakerID];
		string dialogue = packet.dialogueItems[packet.index].text;
		dialogue = DialogueManager.manager.PlaceKeywords(dialogue);
		packet.textObjects[0].text = dialogue;

		if (autoProgress)
		{
			packet.textObjects[0].GetComponent<SideScrollText>().ProgressDialogue = true;
			packet.textObjects[0].GetComponent<SideScrollText>().TimeTillProgess = 1.5f;
			packet.textObjects[0].GetComponent<SideScrollText>().EndDialogue = ShowDialogueOnce;
		}
		
	}

	private void PBSetActive(DialoguePacket packet, bool status)
	{
		packet.images[0].gameObject.SetActive(true);
		packet.images[0].rectTransform.sizeDelta = status ? Vector2.zero : packet.images[0].rectTransform.sizeDelta;
		packet.images[0].gameObject.GetComponent<ResizeBox>().StartResize(this, status ? 0 : 1, BoxChangeSizeTime);

		packet.images[1].gameObject.SetActive(true);
		packet.images[1].rectTransform.sizeDelta = status ? Vector2.zero : packet.images[0].rectTransform.sizeDelta;
		packet.images[1].gameObject.GetComponent<ResizeCharacterProtrait>().PrepareResize(this, status ? 0 : 1, BoxChangeSizeTime / 8);

		packet.images[2].gameObject.SetActive(true);
		packet.images[2].rectTransform.sizeDelta = status ? Vector2.zero : packet.images[0].rectTransform.sizeDelta;
		packet.images[2].gameObject.GetComponent<ResizeBox>().StartResize(this, status ? 0 : 1, BoxChangeSizeTime);
		callbacks = 3;

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
		DialogueManager.manager.SetActive = PBSetActive;
	}
}
