using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DialogueKeywords;
using UnityEngine.Events;
using DialogueFunctionality;

public delegate void SetActiveDelegate(DialoguePacket packet, bool status);

/// <summary>
/// The Amazing class that handles all the dialogue in the game!
/// </summary>
public class DialogueManager : MonoBehaviour {
    //variables
	/// <summary>
	/// The canvas object for the dialogue
	/// </summary>
    public Canvas dialogueCanvas;
	/// <summary>
	/// Choice button prefab
	/// </summary>
	public GameObject CB_Prefab;
	/// <summary>
	/// The index of the currently selected choicebox
	/// </summary>
	public int SelectIndex { get; private set; }
	/// <summary>
	/// The currently selected choicebox
	/// </summary>
	public ChoiceBox SelectedChoiceBox { get; private set; }
	/// <summary>
	/// The data for keywords in dialogue
	/// </summary>
	public KeywordData kw_data;
	/// <summary>
	/// The array of ChoiceButtons
	/// </summary>
    private GameObject[] buttons;
	/// <summary>
	/// The dialogue button data for dialogue
	/// </summary>
    private DialogueButtonData[] choiceOptions;
	/// <summary>
	/// states whether or not buttons will be automaically created if none are provided
	/// </summary>
	private bool createDummyButtons;
	/// <summary>
	/// states whether or not the dialogue relating to the packet is 
	/// </summary>
	private bool isActive;
	/// <summary>
	/// States whether or not the use can press the Enter key
	/// </summary>
	private bool canPress;
	public string[] speakers;
	[SerializeField]
	[Range(0, 60f)]
	private float notificationDuration;
	private bool canClickButton = true;

	/// <summary>
	/// responsible for positioning all of dialogue's buttons
	/// </summary>
	private PositionButtons PositionButtons;
	private OnSelectChoiceBox OnSelectChoiceBox;
	private OnDeselectChoiceBox OnDeselectChoiceBox;

	public IDialogueFunctionality DialogueFunctionality { get; private set; }
	private DialoguePacket MainDialoguePacket;
	public SetActiveDelegate SetActive { private get; set; }

	[SerializeField]
	//private bool ImmediatelyShowButtons = true;
	//private bool showButtons;

	public static DialogueManager manager;

	/// <summary>
	/// Changes the currently selected choicebox
	/// </summary>
	/// <param name="cb">The new selected choicebox</param>
	public void ChangeSelectedChoiceBox(ChoiceBox cb)
	{
		if (SelectedChoiceBox != null)
		{
			OnDeselectChoiceBox();
		}
		SelectedChoiceBox = cb;
		OnSelectChoiceBox();
	}

	private void Awake()
    {
		if (manager != null)
		{
			Destroy(this.gameObject);
		}
		else
		{
			manager = this;
		}

		IDialogueFunctionality dialogueFunctionality = GetComponent<IDialogueFunctionality>();

		MainDialoguePacket = new DialoguePacket();

		MainDialoguePacket.textObjects = dialogueCanvas.GetComponentsInChildren<Text>();
		MainDialoguePacket.images = dialogueCanvas.GetComponentsInChildren<Image>();
		MainDialoguePacket.updateText = dialogueFunctionality.MainUpdateText;
		MainDialoguePacket.updateImages = dialogueFunctionality.MainUpdateImages;
		PositionButtons = dialogueFunctionality.MainPositionButtons;
		OnSelectChoiceBox = dialogueFunctionality.MainOnSelectChoiceBox;
		OnDeselectChoiceBox = dialogueFunctionality.MainOnDeselectChoiceBox;
		MainDialoguePacket.hasCreatedDialogue = false;
		SetActive = SetActiveDefault;

		SetActive(MainDialoguePacket, false);
    }

	/// <summary>
	/// Initates a dialogue sequence at the very beginning of the dialouge
	/// </summary>
	/// <param name="dialogue">The</param>
	public void StartDialogue(Dialogue dialogue)
	{
		StartDialogue(dialogue, 0);
	}

	/// <summary>
	/// Initiates a dialogue sequence at specific point in the dialogue
	/// </summary>
	/// <param name="dialogue">The dialogue to use</param>
	/// <param name="idx">The index in the dialogue to start at</param>
	public void StartDialogue(Dialogue dialogue, int idx)
    {
		//showButtons = ImmediatelyShowButtons;

		SetActive(MainDialoguePacket, true);
		MainDialoguePacket.index = idx;

		MainDialoguePacket.dialogueItems = dialogue.dialogueItems;
		createDummyButtons = dialogue.createDummyButtons;
    }

	/// <summary>
	/// Goes to a specific point in the dialogue
	/// </summary>
	/// <param name="i">The index in the dialogue</param>
	public void GotoIndex(int i)
    {
		if (i < 0)
		{
			EndDialogue();
		}

		MainDialoguePacket.index = i;
		MainDialoguePacket.hasCreatedDialogue = false;
		choiceOptions = null;
		canClickButton = true;
		CloseChoiceBoxes();
	}

	/// <summary>
	/// Goes to the next line of dialogue
	/// </summary>
	public void NextIndex()
    {
        GotoIndex(MainDialoguePacket.index + 1);
	}

	/// <summary>
	/// Ends the conversation
	/// </summary>
	public void EndDialogue()
    {
		CloseChoiceBoxes();
		SetActive(MainDialoguePacket, false);
		MainDialoguePacket.dialogueItems = null;
		choiceOptions = null;
		MainDialoguePacket.hasCreatedDialogue = false;
		canClickButton = true;
	}

	/// <summary>
	/// Updates every frame. Should handle placing keywords and creating outcomes
	/// </summary>
	public void Update()
    {
		canPress = true;
		if (isActive)
        {
			if (MainDialoguePacket.dialogueItems != null)
			{
				if (MainDialoguePacket.index < MainDialoguePacket.dialogueItems.Length)
				{
					if (!MainDialoguePacket.hasCreatedDialogue)
					{
						canPress = false;
						MainDialoguePacket.updateText(MainDialoguePacket);
						MainDialoguePacket.updateImages(MainDialoguePacket);
						choiceOptions = MainDialoguePacket.dialogueItems[MainDialoguePacket.index].dialogueButtons;
						CreateOptions();
						if (buttons != null)
						{
							ChangeSelectedChoiceBox(buttons[SelectIndex].GetComponent<ChoiceBox>());
						}
						MainDialoguePacket.hasCreatedDialogue = true;
					}
				}
				else
				{
					EndDialogue();
				}
				if (buttons != null)
				{
					KeyboardInput();
				}
			}
        }
    }

	/// <summary>
	/// Responsible for constructing the outcome of dialogue
	/// </summary>
	private void CreateOptions()
    {
		SelectIndex = 0;
		if (choiceOptions.Length > 0)
        {
			buttons = new GameObject[choiceOptions.Length];
			for(int i = 0; i < buttons.Length; i++)
			{
				BuildChoiceBox(choiceOptions[i], i);
			}
        }
        else if (createDummyButtons)//if there is no options
        {
			//use dummy option
			buttons = new GameObject[1];
			DialogueButtonData dbd = new DialogueButtonData();
			//sets up button data
			dbd.action = DialogueAction.NextLine;
			dbd.buttonText = "Okay.";
			BuildChoiceBox(dbd, 0);
        }
    }

	/// <summary>
	/// Places keywords into the dialogue's text
	/// </summary>
	/// <param name="text">The line of text to be edited</param>
	/// <returns></returns>
	public string PlaceKeywords(string text)
	{
		int i = 0;
		while (i < text.Length)
		{
			string res = kw_data.PlaceKeyword(i, text, speakers);

			if (res != null)
			{
				text = res;
				i += kw_data.GetLastKeywordLength();
			}
			else
			{
				i++;
			}
		}
		return text;
	}

	/// <summary>
	/// Creates a choicebox
	/// </summary>
	/// <param name="op">The text that displays on the choicebox</param>
	/// <param name="buttonData">The dialogueButtonData for the choicebox</param>
	/// <param name="idx">the index in the buttons array for the choicebox</param>
	private void BuildChoiceBox(DialogueButtonData buttonData, int idx)
	{
		GameObject pf = Instantiate(CB_Prefab);
		pf.GetComponent<ChoiceBox>().txt = PlaceKeywords(buttonData.buttonText);
		pf.GetComponent<ChoiceBox>().buttonData = buttonData;
		pf.transform.SetParent(dialogueCanvas.transform);
		buttons[idx] = pf;

		SelectedChoiceBox = pf.GetComponent<ChoiceBox>();
		OnDeselectChoiceBox();

		PositionButtons(MainDialoguePacket, buttons, idx);

		//pf.SetActive(showButtons);
	}

	private void SetActiveDefault(DialoguePacket packet, bool value)
	{
		foreach(Text textObj in packet.textObjects)
		{
			textObj.gameObject.SetActive(value);
		}

		foreach(Image image in packet.images)
		{
			image.gameObject.SetActive(value);
		}

		isActive = value;
	}

	//public void MakeButtonsVisible()
	//{
	//	foreach(GameObject g in buttons)
	//	{
	//		g.SetActive(true);
	//	}

	//	showButtons = true;
	//}

	/// <summary>
	/// Destroys all current choiceboxes
	/// </summary>
	private void CloseChoiceBoxes()
	{
		if (buttons != null)
		{
			foreach (GameObject g in buttons)
			{
				Destroy(g);
			}
			buttons = null;
			SelectedChoiceBox = null;
		}
	}

	/// <summary>
	/// Responsible for handling keyboard input in dialogues
	/// </summary>
	private void KeyboardInput()
	{
		//if the player presses left or right
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
		{
			//if the player presses "left"
			if (Input.GetKeyDown(KeyCode.A))
			{
				SelectIndex--;
				if (SelectIndex < 0)
				{
					SelectIndex = buttons.Length - 1;
				}
			}
			//if the player presses "right"
			if (Input.GetKeyDown(KeyCode.D))
			{
				SelectIndex++;
				if (SelectIndex >= buttons.Length)
				{
					SelectIndex = 0;
				}
			}

			ChangeSelectedChoiceBox(buttons[SelectIndex].GetComponent<ChoiceBox>());
		}

		//if the user presses enter
		if (PressEnter())
		{
			SelectedChoiceBox.ExecuteAction();
		}
	}

	public void PushNotification(string text)
	{
		string str = PlaceKeywords(text);

		Debug.Log(str); //TODO: change when you got the notification box done
	}

	/// <summary>
	/// Returns whether or not the dialoguebox is active or not
	/// </summary>
	/// <returns></returns>
	public bool DialogueBoxActive
    {
        get
		{
			return isActive;
		}

		set
		{
			isActive = value;
		}
    }

	/// <summary>
	/// Used to make sure there isn't any rapid fires of the Enter key. Use in any instance of the dialogue
	/// </summary>
	/// <returns></returns>
	public bool PressEnter()
	{
		if (Input.GetKeyDown(KeyCode.Return) && canPress)
		{
			canPress = false;
			return true;
		}
		return false;
	}

	public void SetCanClickButton(GameObject obj)
	{
		foreach(GameObject cb in buttons)
		{
			if (cb == obj)
			{
				canClickButton = false;

				return;
			}
		}
	}

	public bool CanClickButton
	{
		get
		{
			return canClickButton;
		}
	}
}
