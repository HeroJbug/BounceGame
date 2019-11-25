using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Does the minimum amount of work to trigger a dialogue to start
/// </summary>
public class DialogueTrigger : MonoBehaviour {
    //variables
    public Dialogue dialogue;

	/// <summary>
	/// Starts the dialogue at beginning of the dialogue
	/// </summary>
	public void InitiateDialogue()
    {
        DialogueManager.manager.StartDialogue(dialogue);
    }

	/// <summary>
	/// Starts the dialogue at a specific point in the dialogue
	/// </summary>
	/// <param name="idx">The index to start the dialogue at</param>
	public void InitiateDialogue(int idx)
	{
		DialogueManager.manager.StartDialogue(dialogue, idx);
	}
}
