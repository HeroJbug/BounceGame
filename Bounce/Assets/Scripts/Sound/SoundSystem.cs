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

	private AudioSource source;

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

		source = GetComponent<AudioSource>();
	}

	public void PlayMusic(string name)
	{
		source.volume = 1;
		source.clip = musicClips[name];

		source.loop = true;
		source.Play();
	}

	public void PlayMusicFadeIn(string name, float time)
	{
		source.clip = musicClips[name];

		source.loop = true;
		source.Play();
		prevVolume = source.volume;
		source.volume = 0;

		effectTime = effectTimeMax = time;

		state = SoundState.FADEIN;
	}

	public void PauseMusic()
	{
		source.Pause();
	}

	public void ResumeMusic()
	{
		source.UnPause();
	}

	public void StopMusicFadeOut(float time)
	{
		prevVolume = source.volume;

		effectTime = effectTimeMax = time;

		state = SoundState.FADEOUT;
	}

	public void StopMusic()
	{
		source.Stop();
	}

    public void PlaySFXMain(string name, float volume)
	{
		source.PlayOneShot(SFXClips[name], volume);
	}

	public void PlaySFX(AudioSource _as, string name, float volume)
	{
		_as.PlayOneShot(SFXClips[name], volume);
	}

	public void PlaySFXLooped(AudioSource _as, string name)
	{
		_as.clip = SFXClips[name];

		_as.Play();
		_as.loop = true;
	}

	public void PlaySFXStopLooped(AudioSource _as)
	{
		_as.Stop();
	}

	private void Update()
	{
		float percentage = Mathf.Clamp01(effectTime / effectTimeMax);

		switch (state)
		{
			case SoundState.FADEIN: //fade music in
				source.volume = prevVolume * (1 - percentage);

				if (effectTime <= 0)
				{
					state = SoundState.NONE;
				}

				effectTime -= Time.deltaTime;
				break;
			case SoundState.FADEOUT: //fade music out
				source.volume = prevVolume * percentage;

				if (effectTime <= 0)
				{
					state = SoundState.NONE;
					source.Stop();
				}

				effectTime -= Time.deltaTime;
				break;
		}

		source.volume = 1;
	}

	public bool IsPlaying()
	{
		return source.isPlaying;
	}

	public bool IsPlaying(AudioSource _as)
	{
		return _as.isPlaying;
	}

	public float PlaybackTime()
	{
		return source.time;
	}

	public float PlaybackTime(AudioSource _as)
	{
		return _as.time;
	}
}
