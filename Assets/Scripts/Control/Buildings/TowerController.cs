using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : BuildingController
{
	private const string TOWERS_PATH = "Models/Buildings/Towers/";

	public TowerBaseController Base;
	public TowerCoreController Core;
	public TowerHeadController Head;

	public Button SellButton;

	protected void Awake()
	{
		GameObject uiCameraObject = GameObject.FindGameObjectWithTag("UI Camera");
		Camera uiCamera = uiCameraObject.GetComponent<Camera>();

		Canvas canvas = this.GetComponentInChildren<Canvas>(true);
		canvas.worldCamera = uiCamera;

		this.BindUpgradesToButtons(BuildingUi.UpgradePanelBase, this.UpgradeTowerBase);
		this.BindUpgradesToButtons(BuildingUi.UpgradePanelCore, this.UpgradeTowerCore);
		this.BindUpgradesToButtons(BuildingUi.UpgradePanelHead, this.UpgradeTowerHead);
	}

	private void BindUpgradesToButtons(GameObject stagePanel, Action<Sprite, Color, GameObject> actionToBind)
	{
		UpgradeButton[] upgradeButtons = stagePanel.GetComponentsInChildren<UpgradeButton>();

		foreach (UpgradeButton upgradeButton in upgradeButtons)
		{
			upgradeButton.OnStageUpgradeRequired += actionToBind;
		}
	}

	public void UpgradeTowerBase(Sprite upgradeButtonSprite, Color upgradeButtonBackground, GameObject basePrefab)
	{
		this.TransfertButtonFormat(BuildingUi.SectionBase, upgradeButtonSprite, upgradeButtonBackground);
		GameObject newBaseObject = Instantiate<GameObject>(basePrefab);
		this.SetBase(newBaseObject);
	}

	public void UpgradeTowerCore(Sprite upgradeButtonSprite, Color upgradeButtonBackground, GameObject corePrefab)
	{
		this.TransfertButtonFormat(BuildingUi.SectionCore, upgradeButtonSprite, upgradeButtonBackground);
		GameObject newCoreObject = Instantiate<GameObject>(corePrefab);
		this.SetCore(newCoreObject);
	}

	public void UpgradeTowerHead(Sprite upgradeButtonSprite, Color upgradeButtonBackground, GameObject headPrefab)
	{
		this.TransfertButtonFormat(BuildingUi.SectionHead, upgradeButtonSprite, upgradeButtonBackground);
		GameObject newHeadObject = Instantiate<GameObject>(headPrefab);
		this.SetHead(newHeadObject);
	}

	private void TransfertButtonFormat(StageSection targetStageSection,  Sprite upgradeButtonSprite, Color upgradeButtonBackground)
	{
		if (upgradeButtonSprite != null && !upgradeButtonBackground.Equals(Color.black))
		{
			targetStageSection.StagePreview.sprite = upgradeButtonSprite;
			targetStageSection.ButtonBackground.color = upgradeButtonBackground;
		}
	}

	public void SetBase(GameObject basePrefab)
	{
		this.DestroyStage(Base);

		GameObject newBaseObject = this.CreateStage(basePrefab);
		Base = newBaseObject.GetComponent<TowerBaseController>();

		this.RelocateStages();
	}

	public void SetCore(GameObject corePrefab)
	{
		this.DestroyStage(Core);

		GameObject newCoreObject = this.CreateStage(corePrefab);
		Core = newCoreObject.GetComponent<TowerCoreController>();

		this.RelocateStages();
	}

	public void SetHead(GameObject headPrefab)
	{
		this.DestroyStage(Head);

		GameObject newHeadObject = this.CreateStage(headPrefab);
		Head = newHeadObject.GetComponent<TowerHeadController>();

		this.RelocateStages();
	}

	private void RelocateStages()
	{
		if (Base != null && Core != null && Head != null)
		{
			Base.transform.localPosition = Vector3.zero;

			float corePosY = this.ComputeStagePosition(new TowerStageController[] { Base });
			Core.transform.localPosition = new Vector3(0, corePosY, 0);

			float headPosY = this.ComputeStagePosition(new TowerStageController[] { Base, Core });
			Head.transform.localPosition = new Vector3(0, headPosY, 0);
		}
	}

	public void DestroyStage(TowerStageController stage)
	{
		if (stage != null)
		{
			DestroyImmediate(stage.gameObject, true);
		}
	}

	private GameObject CreateStage(GameObject stageObject)
	{
		stageObject.transform.SetParent(transform);
		return stageObject;
	}

	private float ComputeStagePosition(TowerStageController[] stagesBellow)
	{
		float position = 0;

		foreach (TowerStageController currentStage in stagesBellow)
		{
			if (currentStage != null)
			{
				position += currentStage.Height;
			}
		}

		return position;
	}
}
