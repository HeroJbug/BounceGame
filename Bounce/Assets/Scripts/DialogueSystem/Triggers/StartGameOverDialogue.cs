using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameOverDialogue : MonoBehaviour
{
	private DialogueTrigger trig;
	[SerializeField]
	private float messageDelay;
	private float timeTillFire;
	private bool firedMessage = false;

    // Start is called before the first frame update
    void Awake()
    {
		trig = GetComponent<DialogueTrigger>();
    }

	private void Update()
	{
		if (!firedMessage)
		{
			if (timeTillFire >= messageDelay)
			{
				FireGameOverMessage();
				firedMessage = true;
			}
			else
			{
				timeTillFire += Time.deltaTime;
			}
		}
	}

	private void FireGameOverMessage()
	{
		int idx = Random.Range(0, trig.dialogue.dialogueItems.Length - 1);

		trig.InitiateDialogue(idx);
	}
}
