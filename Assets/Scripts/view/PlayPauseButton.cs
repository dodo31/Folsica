using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPauseButton : ToggleButton
{
	public PlayPauseSprites playPauseSprites;

	protected void Start()
	{
		this.SetAsPause();
	}

	public void SetAsPlay()
	{
		IconImage.sprite = playPauseSprites.PlaySprite;
	}

	public void SetAsPause()
	{
		IconImage.sprite = playPauseSprites.PauseSprite;
	}
}
