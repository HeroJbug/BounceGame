using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundDictionary : UDictionary<string, AudioClip> { }

[SerializeField]
public enum SoundState
{
	NONE,
	FADEOUT,
	FADEIN
}


public class SoundSystem : MonoBehaviour
{
	[SerializeField]
	private SoundDictionary musicClips;

	[SerializeField]
	private SoundDictionary SFXClips;

	private AudioSource[] sources;

	private SoundState state = SoundState.NONE;
	private float effectTimeMax;
	private float effectTime;
	private float prevVolume;

	public static SoundSystem system;

	// Start is called before the first frame update
	void Awake()
	{
		if (system == null)
		{
			system = this;
			DontDestroyOnLoad(system);
		}
		else
		{
			Destroy(this.gameObject);
		}

		sources = GetComponents<AudioSource>();
		sources[0].priority = 0;
		sources[1].priority = 1;
	}

	public void PlayMusic(string name)
	{
		sources[0].volume = 1;
		sources[0].clip = musicClips[name];

		sources[0].loop = true;
		sources[0].Play();
	}

	public void PlayMusicFadeIn(string name, float time)
	{
		sources[0].clip = musicClips[name];

		sources[0].loop = true;
		sources[0].Play();
		prevVolume = sources[0].volume;
		sources[0].volume = 0;

		effectTime = effectTimeMax = time;

		state = SoundState.FADEIN;
	}

	public void PauseMusic()
	{
		sources[0].Pause();
	}

	public void ResumeMusic()
	{
		sources[0].UnPause();
	}

	public void StopMusicFadeOut(float time)
	{
		prevVolume = sources[0].volume;

		effectTime = effectTimeMax = time;

		state = SoundState.FADEOUT;
	}

	public void StopMusic()
	{
		sources[0].Stop();
	}

    public void PlaySFX(string name, float volume)
	{
		sources[1].PlayOneShot(SFXClips[name], volume);
	}

	public void PlaySFXLooped(string name)
	{
		sources[1].clip = SFXClips[name];

		sources[1].Play();
		sources[1].loop = true;
	}

	public void PlaySFXStopLooped()
	{
		sources[1].Stop();
	}

	private void Update()
	{
		float percentage = Mathf.Clamp01(effectTime / effectTimeMax);

		switch (state)
		{
			case SoundState.FADEIN: //fade music in
				sources[0].volume = prevVolume * (1 - percentage);

				if (effectTime <= 0)
				{
					state = SoundState.NONE;
				}

				effectTime -= Time.deltaTime;
				break;
			case SoundState.FADEOUT: //fade music out
				sources[0].volume = prevVolume * percentage;

				if (effectTime <= 0)
				{
					state = SoundState.NONE;
					sources[0].Stop();
				}

				effectTime -= Time.deltaTime;
				break;
		}

		sources[1].volume = 1;
	}

	public bool IsPlaying(int idx)
	{
		return sources[idx].isPlaying;
	}

	public float PlaybackTime
	{
		get
		{
			return sources[0].time;
		}
	}
}
