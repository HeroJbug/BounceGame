using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayMusic : MonoBehaviour
{
	public string SongToPlay;
	
	// Start is called before the first frame update
    void Start()
    {
		SoundSystem.system.PlayMusic(SongToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
