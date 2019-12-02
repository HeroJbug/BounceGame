using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueFunctionality
{
	public delegate void UpdateImages(DialoguePacket packet);
	public delegate void UpdateText(DialoguePacket packet);
	public delegate void PositionButtons(DialoguePacket packet, GameObject[] buttons, int idx);
	public delegate void OnSelectChoiceBox();
	public delegate void OnDeselectChoiceBox();

	[System.Serializable]
	public class DialoguePacket
	{
		/// <summary>
		/// the index the dialogue is at
		/// </summary>
		public int index;
		/// <summary>
		/// the items used in dialogue
		/// </summary>
		public DialogueItem[] dialogueItems;
		/// <summary>
		/// the images used in dialogue
		/// </summary>
		public Image[] images;
		/// <summary>
		/// the Text objects that are used to display various kinds of text
		/// </summary>
		public Text[] textObjects;
		/// <summary>
		/// updates any images used in the dialogue
		/// </summary>
		public UpdateImages updateImages;
		/// <summary>
		/// updates any text used in the dialogue
		/// </summary>
		public UpdateText updateText;
		/// <summary>
		/// used to see if dialogue has been created yet
		/// </summary>
		public bool hasCreatedDialogue;
	}

	public interface IDialogueFunctionality
	{
		void MainUpdateImages(DialoguePacket packet);
		void MainUpdateText(DialoguePacket packet);
		void MainPositionButtons(DialoguePacket packet, GameObject[] buttons, int idx);

		void MainOnSelectChoiceBox();
		void MainOnDeselectChoiceBox();

		
	}
}
