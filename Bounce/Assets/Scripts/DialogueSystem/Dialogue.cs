using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueAction
{
	NextLine,
	JumpToLine,
	EndDialogue
}

[System.Serializable]
public struct DialogueButtonData
{
	public DialogueAction action;
	public int line;
	public int altLine;
	public string buttonText;
}

[System.Serializable]
public struct DialogueItem
{
	public int speakerID;
	public int charPortraitID;
	[TextArea(1, 10)]
	public string text;
	public DialogueButtonData[] dialogueButtons;
	
};

[System.Serializable]
public class Dialogue {
	//variables
	/// <summary>
	/// states whether or not buttons will be automaically created if none are provided
	/// </summary>
	public bool createDummyButtons;
	public DialogueItem[] dialogueItems;
}
