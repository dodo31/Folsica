using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccelerateButton : ToggleButton
{
	public AccelerateNormalSprites AccelerateNormalSprites;

	protected void Start()
	{
        this.SetAsNormal();
	}

	public void SetAsAccelerate()
	{
		IconImage.sprite = AccelerateNormalSprites.AccelerateSprite;
	}

	public void SetAsNormal()
	{
		IconImage.sprite = AccelerateNormalSprites.NormalSprite;
	}
}
