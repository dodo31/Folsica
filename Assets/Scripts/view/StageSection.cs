using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSection : MonoBehaviour
{
	public Text PowerIndicator;
	public Text RangeIndicator;
	public Text SpeedIndicator;

	public Button SelectUpgradeButton;

	public Image StagePreview;
	public Image ButtonBackground;

	protected void Start()
	{

	}

	public void SetPower(int newPower)
	{
		PowerIndicator.text = newPower.ToString();
	}

	public void SetRange(int newRange)
	{
		RangeIndicator.text = newRange.ToString();
	}

	public void SetSpeed(int newSpeed)
	{
		SpeedIndicator.text = newSpeed.ToString();
	}
}
