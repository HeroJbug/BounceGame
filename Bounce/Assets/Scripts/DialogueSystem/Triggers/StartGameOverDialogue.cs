using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameOverDialogue : MonoBehaviour
{
	private DialogueTrigger trig;
	[SerializeField]
	private float messageDelay;

    // Start is called before the first frame update
    void Start()
    {
		trig = GetComponent<DialogueTrigger>();

		Invoke("FireGameOverMessage", messageDelay);
    }

    private void FireGameOverMessage()
	{
		int idx = Random.Range(0, trig.dialogue.dialogueItems.Length - 1);

		trig.InitiateDialogue(idx);
	}
}
