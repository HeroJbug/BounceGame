using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScreenTrigger : MonoBehaviour
{
	private DialogueTrigger trig;
	public GameObject tutorialEnemyPrefab;
	private EnemySpawner spawner;
	private GameObject player;
	private bool firedMessage;
	private Enemy tutorialEnemy;
	[SerializeField]
	private int state = 0;
	[SerializeField]
	private Button button;

	public struct MoveKeysPressed
	{
		public bool pressedUp;
		public bool pressedDown;
		public bool pressedRight;
		public bool pressedLeft;
	}

	public MoveKeysPressed moveKeysPressed;

	private void RemoveDeadEnemies(GameObject e)
	{
		//does nothing since it's just a placeholder
	}

	private void OnEnable()
	{
		Enemy.EnemyDeathEvent += RemoveDeadEnemies;
	}

	private void OnDisable()
	{
		Enemy.EnemyDeathEvent -= RemoveDeadEnemies;
	}

	// Start is called before the first frame update
	void Start()
    {
		spawner = FindObjectOfType<EnemySpawner>();
		trig = GetComponent<DialogueTrigger>();
		trig.InitiateDialogue(0);
		player = FindObjectOfType<PlayerMovement>().gameObject;
	}

    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.manager.DialogueBoxActive)
		{
			button.gameObject.SetActive(false);
			if (!firedMessage)
			{
				switch (state)
				{
					case 0: //if the player needs to learn how to move
						DetectMoveInputs();

						if (moveKeysPressed.pressedLeft && moveKeysPressed.pressedRight && moveKeysPressed.pressedUp && moveKeysPressed.pressedDown)
						{
							trig.InitiateDialogue(27);
							state++;
							firedMessage = true;
						}
						break;

					case 1: //if the player needs to learn how to dash
						if (Input.GetButtonDown("Dash"))
						{
							trig.InitiateDialogue(28);
							state++;
							firedMessage = true;
							tutorialEnemy = spawner.SpawnEnemy(tutorialEnemyPrefab, player).GetComponent<Enemy>();
							tutorialEnemy.enabled = false;
						}
						break;

					case 2: //if the player needs to learn how to take down an enemy.
						if (tutorialEnemy == null)
						{
							trig.InitiateDialogue(32);
							state++;
							firedMessage = true;
						}
						else
						{
							tutorialEnemy.enabled = true;
						}
						break;

					default: //if the tutorial has finished
						SceneManager.LoadScene(0);
						break;
				}
			}
		}
		else
		{
			button.gameObject.SetActive(true);
			firedMessage = false;
		}
    }

	private void DetectMoveInputs()
	{
		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");

		if (inputX > 0)
		{
			moveKeysPressed.pressedRight = true;
		}

		if (inputX < 0)
		{
			moveKeysPressed.pressedLeft = true;
		}

		if (inputY > 0)
		{
			moveKeysPressed.pressedUp = true;
		}

		if (inputY < 0)
		{
			moveKeysPressed.pressedDown = true;
		}
	}
}
