using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnd : MonoBehaviour
{
	private DialogueTrigger trig;

	[SerializeField]
	private SpawnerManager spawnerManager;
	
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
