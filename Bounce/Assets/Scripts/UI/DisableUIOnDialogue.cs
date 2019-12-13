using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUIOnDialogue : MonoBehaviour
{
	private PlayerMovement player;
	private bool currentStatus = false;
	[SerializeField]
	private List<GameObject> keepDisabled;
	
	// Start is called before the first frame update
    void Start()
    {
		player = FindObjectOfType<PlayerMovement>();

		if (!player.isInTutorialMode)
		{
			this.enabled = false;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.manager.DialogueBoxActive && currentStatus == false)
		{
			for(int i = 0; i < transform.childCount; i++)
			{
				if (!keepDisabled.Contains(transform.GetChild(i).gameObject))
					transform.GetChild(i).gameObject.SetActive(false);
			}

			currentStatus = true;
		}
		else if (!DialogueManager.manager.DialogueBoxActive && currentStatus == true)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				if (!keepDisabled.Contains(transform.GetChild(i).gameObject))
					transform.GetChild(i).gameObject.SetActive(true);
			}

			currentStatus = false;
		}
    }
}
