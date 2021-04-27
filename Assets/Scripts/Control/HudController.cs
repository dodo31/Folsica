using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
	public EconomyController EconomyController;
	public GlobalHealthController GlobalHealthController;

	public Text HumanResourceAmount;
	public Text AlienResourceAmount;
	public Text RobotResourceAmount;

	public Text CurrentDay;

	public Text Health;

	private Economy economy;
	private GlobalHealth globalHealth;


	private void Start()
	{
		economy = Economy.GetInstance();
		globalHealth = GlobalHealth.GetInstance();

		EconomyController.OnTransaction += (float amount, Race resourceType) =>
		{
			this.UpdateResources();
		};
		this.UpdateResources();

		GlobalHealthController.OnHealthDecreased += (float health) =>
		{
			this.UpdateHp();
		};
		this.UpdateHp();
	}

	private void FixedUpdate()
	{
		economy.GainResource(0.1f, Race.HUMAN);
		economy.GainResource(0.1f, Race.ALIEN);
		economy.GainResource(0.1f, Race.ROBOT);
		this.UpdateResources();
	}

	private void UpdateResources()
	{
		HumanResourceAmount.text = Mathf.Round(economy.HumanResourceAmout).ToString();
		AlienResourceAmount.text = Mathf.Round(economy.AlienResourceAmout).ToString();
		RobotResourceAmount.text = Mathf.Round(economy.RobotResourceAmout).ToString();
	}

	public void SetDay(int newDay)
	{
		CurrentDay.text = newDay.ToString();
	}

	public void UpdateHp()
	{
		int totalHp = Mathf.FloorToInt(globalHealth.TotalHealth);
		int remainingHp = Mathf.FloorToInt(globalHealth.RemainingHealth);
		Health.text = remainingHp + " / " + totalHp;
	}
}
