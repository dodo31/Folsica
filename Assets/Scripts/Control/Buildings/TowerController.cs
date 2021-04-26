using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : BuildingController
{
	private const string TOWERS_PATH = "Models/Buildings/Towers/";
	public float RotationSpeed = 0.01f;

	public TowerBaseController Base;
	public TowerCoreController Core;
	public TowerHeadController Head;

	public Button SellButton;

	public Text SellAmout;

	private float lastFireTime;

	private float currentAngle;
	private Vector3 targetDirection;

	private GeometryHelper geometryHelper;

	protected void Awake()
	{
		GameObject uiCameraObject = GameObject.FindGameObjectWithTag("UI Camera");
		Camera uiCamera = uiCameraObject.GetComponent<Camera>();

		Canvas canvas = this.GetComponentInChildren<Canvas>(true);
		canvas.worldCamera = uiCamera;

		this.BindUpgradesToButtons(BuildingUi.UpgradePanelBase, this.UpgradeTowerBase);
		this.BindUpgradesToButtons(BuildingUi.UpgradePanelCore, this.UpgradeTowerCore);
		this.BindUpgradesToButtons(BuildingUi.UpgradePanelHead, this.UpgradeTowerHead);

		lastFireTime = Time.time;

		currentAngle = 0;
		targetDirection = Vector3.positiveInfinity;

		geometryHelper = new GeometryHelper();
	}

	private void BindUpgradesToButtons(GameObject stagePanel, Action<Sprite, Color, GameObject> actionToBind)
	{
		UpgradeButton[] upgradeButtons = stagePanel.GetComponentsInChildren<UpgradeButton>();

		foreach (UpgradeButton upgradeButton in upgradeButtons)
		{
			upgradeButton.OnStageUpgradeRequired += actionToBind;
		}
	}

	protected void LateUpdate()
	{
		float currentTime = Time.time;

		if (currentTime - lastFireTime > this.TotalCadence())
		{
			// Fire

			targetDirection = Head.SelectTargetDirection(this.TotalRange());

			if (targetDirection.x != float.PositiveInfinity)
			{
				this.RefreshGunDirection(targetDirection);
			}

			lastFireTime = currentTime;
		}
		else
		{
			if (targetDirection.x == float.PositiveInfinity)
			{
				targetDirection = Head.SelectTargetDirection(this.TotalRange());
			}

			if (targetDirection.x != float.PositiveInfinity)
			{
				this.RefreshGunDirection(targetDirection);
			}
		}
	}

	private float TotalPowerMultiplicator()
	{
		float totalMultiplicator = Head.PowerMultiplicator;

		if (Base is TowerBaseHumanController baseHuman)
		{
			totalMultiplicator *= baseHuman.PowerMultiplicator;
		}

		return totalMultiplicator;
	}

	private float TotalCadence()
	{
		float totalCadence = Head.Cadence;

		if (Base is TowerBaseAlienController baseAlien)
		{
			totalCadence /= baseAlien.CadenceMultiplicator;
		}

		return totalCadence;
	}

	private float TotalRange()
	{
		float totalRange = Core.Range;

		if (Base is TowerBaseRobotController baseRobot)
		{
			totalRange *= baseRobot.RangeMultiplicator;
		}

		return totalRange;
	}

	private void RefreshGunDirection(Vector3 targetDirection)
	{
		Vector3 coreRotation = Core.transform.rotation.eulerAngles;
		Vector3 headRotation = Head.transform.rotation.eulerAngles;

		float targetAngle = Mathf.Atan2(targetDirection.z, targetDirection.x);
		float angleDelta = geometryHelper.AngleDelta(targetAngle, currentAngle);

		float angleIncrement = -Math.Sign(angleDelta) * Mathf.Min(Mathf.Abs(angleDelta), RotationSpeed);
		float nextAngle = currentAngle + angleIncrement;
		currentAngle = nextAngle;

		float nextDirX = Mathf.Cos(nextAngle);
		float nextDirY = Mathf.Sin(nextAngle);
		Vector3 nextDirection = new Vector3(nextDirX, 0, nextDirY);

		Core.transform.rotation = Quaternion.FromToRotation(transform.right, nextDirection);
		Head.transform.rotation = Core.transform.rotation;
	}

	public void UpgradeTowerBase(Sprite upgradeButtonSprite, Color upgradeButtonBackground, GameObject basePrefab)
	{
		this.TransfertButtonFormat(BuildingUi.SectionBase, upgradeButtonSprite, upgradeButtonBackground);
		GameObject newBaseObject = Instantiate<GameObject>(basePrefab);
		this.SetBase(newBaseObject);
		this.RefreshSellPrice();
	}

	public void UpgradeTowerCore(Sprite upgradeButtonSprite, Color upgradeButtonBackground, GameObject corePrefab)
	{
		this.TransfertButtonFormat(BuildingUi.SectionCore, upgradeButtonSprite, upgradeButtonBackground);
		GameObject newCoreObject = Instantiate<GameObject>(corePrefab);
		this.SetCore(newCoreObject);
		this.RefreshSellPrice();
	}

	public void UpgradeTowerHead(Sprite upgradeButtonSprite, Color upgradeButtonBackground, GameObject headPrefab)
	{
		this.TransfertButtonFormat(BuildingUi.SectionHead, upgradeButtonSprite, upgradeButtonBackground);
		GameObject newHeadObject = Instantiate<GameObject>(headPrefab);
		this.SetHead(newHeadObject);
		this.RefreshSellPrice();
	}

	private void RefreshSellPrice()
	{
		SellAmout.text = this.TotalPrice() + " $";
	}

	private float TotalPrice()
	{
		float basePrice = Base != null ? Base.Price : 0;
		float corePrice = Core != null ? Core.Price : 0;
		float headPrice = Head != null ? Head.Price : 0;
		return basePrice + corePrice + headPrice;
	}

	private void TransfertButtonFormat(StageSection targetStageSection, Sprite upgradeButtonSprite, Color upgradeButtonBackground)
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
