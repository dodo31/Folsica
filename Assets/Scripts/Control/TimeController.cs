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
			isPlaying = false;
			playPauseButton.SetAsPlay();
			accelerateButton.ToggleInterractable(false);
		}
		else
		{
			isPlaying = true;
			playPauseButton.SetAsPause();
			accelerateButton.ToggleInterractable(true);
		}
	}

	public void ToggleAccelerate()
	{
		if (isAccelerated)
		{
			isAccelerated = false;
			accelerateButton.SetAsNormal();
		}
		else
		{
			if (isPlaying)
			{
				isAccelerated = true;
				accelerateButton.SetAsAccelerate();
			}
		}
	}
}