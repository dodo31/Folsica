using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
	// public string StagePath;

	public GameObject StagePrefab;

	public Image StagePreview;
	public Image ButtonBackground;
	public Text PriceText;

	public event Action<Sprite, Color, GameObject> OnStageUpgradeRequired;

	protected void Awake()
	{
		TowerStageController relatedStage = StagePrefab.GetComponent<TowerStageController>();
		this.SetPrice(relatedStage.Price);
	}

	public void SetPrice(int newPrice)
	{
		PriceText.text = newPrice + " $";
	}

	public void TriggerUpgrade()
	{
		OnStageUpgradeRequired.Invoke(StagePreview.sprite, ButtonBackground.color, StagePrefab);
	}
}
