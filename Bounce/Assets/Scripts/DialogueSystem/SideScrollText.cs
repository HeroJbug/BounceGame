using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideScrollText : MonoBehaviour
{
	private Text textObj;
	[SerializeField]
	private float textSpeed;
	private string textToPlace = "";
	private float charsToDisplay;
	public bool ProgressDialogue { get; set; }
	public bool EndDialogue { get; set; }
	public float TimeTillProgess { get; set; }
	private float ttp_counter;
	private bool canProgress = false;
	public bool playTalkSound;
	public string talkSoundName;
	private AudioSource parentSource;
	
	// Start is called before the first frame update
    void Start()
    {
		textObj = GetComponent<Text>();

		parentSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		if (DialogueManager.manager.DialogueBoxActive)
		{
			if (Mathf.RoundToInt(charsToDisplay) != textObj.text.Length || (textToPlace.Length == textObj.text.Length && textObj.text != textToPlace))
			{
				textToPlace = textObj.text;
				textObj.text = "";
				charsToDisplay = 0;
			}

			if (Mathf.RoundToInt(charsToDisplay) != textToPlace.Length)
			{
				if (playTalkSound && !parentSource.isPlaying)
				{
					SoundSystem.system.PlaySFXLooped(parentSource, talkSoundName);
				}

				charsToDisplay = Mathf.Clamp(charsToDisplay + (textSpeed * Time.deltaTime), 0, textToPlace.Length);
				textObj.text = textToPlace.Substring(0, Mathf.RoundToInt(charsToDisplay));

				if (Mathf.RoundToInt(charsToDisplay) == textToPlace.Length)
				{
					ttp_counter = TimeTillProgess;
					SoundSystem.system.StopSFXLooped(parentSource);
					canProgress = true;
				}
			}

			if (ProgressDialogue && canProgress)
			{
				if (ttp_counter <= 0)
				{
					canProgress = false;
					ttp_counter = TimeTillProgess;
					if (!EndDialogue)
					{
						DialogueManager.manager.NextIndex();
					}
					else
					{
						DialogueManager.manager.EndDialogue();
					}
				}
				else
				{
					ttp_counter -= Time.deltaTime;
				}
			}
		}
	}

	public void OnDisable()
	{
		if (parentSource != null)
		{
			SoundSystem.system.StopSFXLooped(parentSource);
		}
	}
}
