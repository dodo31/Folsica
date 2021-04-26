using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
	public PlayPauseButton playPauseButton;
	public AccelerateButton accelerateButton;

	private bool isPlaying;
	private bool isAccelerated;

	protected void Start()
	{
		isPlaying = true;
		isAccelerated = false;
	}

	public void TogglePlayPause()
	{
		if (isPlaying)
		{
			Time.timeScale = 0;

			isPlaying = false;
			playPauseButton.SetAsPlay();
			accelerateButton.ToggleInterractable(false);
		}
		else
		{
			if (isAccelerated)
			{
				Time.timeScale = 2;
			}
			else
			{
				Time.timeScale = 1;
			}

			isPlaying = true;
			playPauseButton.SetAsPause();
			accelerateButton.ToggleInterractable(true);
		}
	}

	public void ToggleAccelerate()
	{
		if (isAccelerated)
		{
			Time.timeScale = 1;

			isAccelerated = false;
			accelerateButton.SetAsNormal();
		}
		else
		{
			if (isPlaying)
			{
				Time.timeScale = 2;

				isAccelerated = true;
				accelerateButton.SetAsAccelerate();
			}
		}
	}
}