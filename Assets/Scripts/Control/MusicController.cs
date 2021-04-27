using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	public AudioSource MainAudioSource;
	public AudioClip MainStart;
	public AudioClip MainLoop;

	public AudioSource NoBatteryAudioSource;

	public AudioClip NoBatteryStart;
	public AudioClip NoBatteryLoop;

	protected void Awake()
	{
		MainAudioSource.clip = MainStart;
		MainAudioSource.Play();

		NoBatteryAudioSource.clip = NoBatteryStart;
		NoBatteryAudioSource.Play();

		this.ToggleBattery(true);
	}

	protected void Update()
	{
		if (!MainAudioSource.isPlaying && !NoBatteryAudioSource.isPlaying)
		{
			MainAudioSource.clip = MainLoop;
			MainAudioSource.loop = true;
			MainAudioSource.Play();

			NoBatteryAudioSource.clip = NoBatteryLoop;
			NoBatteryAudioSource.loop = true;
			NoBatteryAudioSource.Play();
		}
	}

	public void ToggleBattery(bool enableBattery)
	{
		if (enableBattery)
		{
			MainAudioSource.mute = false;
			NoBatteryAudioSource.mute = true;
		}
		else
		{
			MainAudioSource.mute = true;
			NoBatteryAudioSource.mute = false;
		}
	}
}
