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

	public event Action<float, Race> OnUpgradeConsummed;

	private float lastFireTime;

	private float currentAngle;
	private Vector3 targetDirection;

	private GeometryHelper geometryHelper;

	private Economy economy;

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

		economy = Economy.GetInstance();
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
		if (!isOnMove && this.IsComplete())
		{
			float currentTime = Time.time;

			float cadence = this.TotalCadence();

			if (cadence > 0 && currentTime - lastFireTime > cadence)
			{
				float totalRange = this.TotalRange();

				if (Core.ProjectilePrefab != null)
				{
					this.Fire(totalRange);
				}

				targetDirection = Head.SelectTargetDirection(totalRange);

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

	private void Fire(float totalRange)
	{
		float powerMultiplicator = this.TotalPowerMultiplicator();

		float directionX = Mathf.Cos(currentAngle);
		float directionY = Mathf.Sin(currentAngle);
		Vector3 currentdDrection = new Vector3(directionX, 0, directionY);

		GameObject newProjectileObject = GameObject.Instantiate<GameObject>(Core.ProjectilePrefab);
		newProjectileObject.transform.SetParent(transform.parent);

		ProjectileController newProjectile = newProjectileObject.GetComponent<ProjectileController>();
		newProjectile.Emit(currentdDrection, Core.transform.position, totalRange, powerMultiplicator);
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
		this.UpgradeStage(upgradeButtonSprite, upgradeButtonBackground, basePrefab, this.SetBase);
	}

	public void UpgradeTowerCore(Sprite upgradeButtonSprite, Color upgradeButtonBackground, GameObject corePrefab)
	{
		this.UpgradeStage(upgradeButtonSprite, upgradeButtonBackground, corePrefab, this.SetCore);
	}

	public void UpgradeTowerHead(Sprite upgradeButtonSprite, Color upgradeButtonBackground, GameObject headPrefab)
	{
		this.UpgradeStage(upgradeButtonSprite, upgradeButtonBackground, headPrefab, this.SetHead);
	}

	private void UpgradeStage(Sprite upgradeButtonSprite, Color upgradeButtonBackground, GameObject stagePrefab, Action<GameObject> stageSetter)
	{
		if (this.CanUpgrade(stagePrefab))
		{
			this.TransfertButtonFormat(BuildingUi.SectionHead, upgradeButtonSprite, upgradeButtonBackground);
			GameObject newStageObject = Instantiate<GameObject>(stagePrefab);
			stageSetter(newStageObject);
			this.RefreshSellPrice();
			this.PropagateUpgradeCost(stagePrefab);
		}
	}

	private bool CanUpgrade(GameObject stagePrefab)
	{
		TowerStageController stage = stagePrefab.GetComponent<TowerStageController>();

		switch (stage.Race)
		{
		case Race.HUMAN:
			return stage.Price <= economy.HumanResourceAmout;
		case Race.ALIEN:
			return stage.Price <= economy.AlienResourceAmout;
		case Race.ROBOT:
			return stage.Price <= economy.RobotResourceAmout;
		case Race.NEUTRAL:
			return true;
		}

		return false;
	}

	private void RefreshSellPrice()
	{
		SellAmout.text = this.TotalPrice() + " $";
	}

	private void PropagateUpgradeCost(GameObject stagePrefab)
	{
		TowerStageController stage = stagePrefab.GetComponent<TowerStageController>();
		OnUpgradeConsummed.Invoke(stage.Price, stage.Race);
	}

	private float TotalPrice()
	{
		float basePrice = Base != null ? Base.Price : 0;
		float corePrice = Core != null ? Core.Price : 0;
		float headPrice = Head != null ? Head.Price : 0;
		return basePrice + corePrice + headPrice;
	}

	public float TotalHumanResourceValue()
	{
		float totalHumanResourceAmount = 0;
		totalHumanResourceAmount += (Base.Race == Race.HUMAN ? Base.Price : 0);
		totalHumanResourceAmount += (Core.Race == Race.HUMAN ? Core.Price : 0);
		totalHumanResourceAmount += (Head.Race == Race.HUMAN ? Head.Price : 0);
		return totalHumanResourceAmount;
	}

	public float TotalAlienResourceValue()
	{
		float totalAlienResourceAmount = 0;
		totalAlienResourceAmount += (Base.Race == Race.ALIEN ? Base.Price : 0);
		totalAlienResourceAmount += (Core.Race == Race.ALIEN ? Core.Price : 0);
		totalAlienResourceAmount += (Head.Race == Race.ALIEN ? Head.Price : 0);
		return totalAlienResourceAmount;
	}

	public float TotalRobotResourceValue()
	{
		float totalRobotResourceAmount = 0;
		totalRobotResourceAmount += (Base.Race == Race.ROBOT ? Base.Price : 0);
		totalRobotResourceAmount += (Core.Race == Race.ROBOT ? Core.Price : 0);
		totalRobotResourceAmount += (Head.Race == Race.ROBOT ? Head.Price : 0);
		return totalRobotResourceAmount;
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
		if (this.IsComplete())
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

	private bool IsComplete()
	{
		return Base != null && Core != null && Head != null;
	}
}
