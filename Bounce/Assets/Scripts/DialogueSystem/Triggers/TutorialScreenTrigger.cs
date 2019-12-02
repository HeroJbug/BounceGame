using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreenTrigger : MonoBehaviour
{
	private DialogueTrigger trig;
	public Enemy tutorialEnemyPrefab;
	public Vector2 tutorialEnemySpawnPoint;
	
	// Start is called before the first frame update
    void Start()
    {
		trig = GetComponent<DialogueTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
