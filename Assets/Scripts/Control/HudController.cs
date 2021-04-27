using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
	public EconomyController EconomyController;

	public Text HumanResourceAmount;
	public Text AlienResourceAmount;
	public Text RobotResourceAmount;

	private Economy economy;


	private void Awake()
	{
		economy = Economy.GetInstance();
		EconomyController.OnTransaction += (float amount, Race resourceType) =>
		{
			this.UpdateResources();
		};
		this.UpdateResources();
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
}
