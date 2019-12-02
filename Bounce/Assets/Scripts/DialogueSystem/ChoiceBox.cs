using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// The button that handles actions in dialogue
/// </summary>
public class ChoiceBox : MonoBehaviour {
	/// <summary>
	/// The text of the ChoiceBox
	/// </summary>
	public string txt = "";
	/// <summary>
	/// The action data of the ChoiceBox
	/// </summary>
    public DialogueButtonData buttonData;
	/// <summary>
	/// The button portion of the ChoiceBox
	/// </summary>
    private Button button;
	/// <summary>
	/// The text of the ChoiceBox's button
	/// </summary>
    private Text text;

	private Vector2 size;

    void Awake()
    {
		EventTrigger.Entry mouseOverBox = new EventTrigger.Entry();
		mouseOverBox.eventID = EventTriggerType.PointerEnter;
		mouseOverBox.callback.AddListener(TaskOnPointerEnter);

		button = GetComponent<Button>();
		text = button.GetComponentInChildren<Text>();
        button.onClick.AddListener(TaskOnClick);
		GetComponent<EventTrigger>().triggers.Add(mouseOverBox);

		size = button.image.rectTransform.sizeDelta;
    }

    // Update is called once per frame
    void Update () {
		if (text != null)
		{
			text.text = txt;
		}
	}

	/// <summary>
	/// Changes a ChoiceBox's color
	/// </summary>
	/// <param name="c">The new color of the ChoiceBox</param>
	public void ChangeColor(Color normalColor, Color highlightedColor, Color pressedColor)
	{
		var newColors = button.colors;

		newColors.normalColor = normalColor;
		newColors.highlightedColor = highlightedColor;
		newColors.pressedColor = pressedColor;

		button.colors = newColors;
	}

	/// <summary>
	/// Changes a ChoiceBox's size
	/// </summary>
	/// <param name="amt">The new size of the ChoiceBox</param>
	public void ChangeSize(float amt)
	{
		button.image.rectTransform.sizeDelta = size * amt;
	}

	public void ChangeSprite(Sprite sprite)
	{
		button.image.sprite = sprite;
	}

	/// <summary>
	/// If the mouse cursor falls on top of a button
	/// </summary>
	/// <param name="e">The event data</param>
	private void TaskOnPointerEnter(BaseEventData e)
	{
		DialogueManager.manager.ChangeSelectedChoiceBox(this);
	}

	/// <summary>
	/// The task that is performed if the ChoiceBox is selected
	/// </summary>
	private void TaskOnClick()
    {
		if (DialogueManager.manager.CanClickButton)
		{
			DialogueManager.manager.SetCanClickButton(this.gameObject);

			switch (buttonData.action)
			{
				case DialogueAction.EndDialogue: //if the ChoiceBox's action is to end the dialogue
					DialogueManager.manager.EndDialogue();
					break;
				case DialogueAction.JumpToLine: //if the ChoiceBox's action is to jump to a specific point in dialogue
					DialogueManager.manager.GotoIndex(buttonData.line);
					break;
				case DialogueAction.NextLine: //if the ChoiceBox's action is to jump to the next point in dialogue
					DialogueManager.manager.NextIndex();
					break;
			}
		}
	}

	public void ExecuteAction()
	{
		TaskOnClick();
	}
}
