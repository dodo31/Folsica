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

	public Image FadeOverlay;

	public event Action OnFadeOutCompleted;

	private Economy economy;
	private GlobalHealth globalHealth;

	private bool isFadingOut;

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

		isFadingOut = false;
	}

	private void FixedUpdate()
	{
		economy.GainResource(0.1f, Race.HUMAN);
		economy.GainResource(0.1f, Race.ALIEN);
		economy.GainResource(0.1f, Race.ROBOT);
		this.UpdateResources();

		if (isFadingOut)
		{
			float currentOverlayAlpha = FadeOverlay.color.a;
			FadeOverlay.color = new Color(0, 0, 0, currentOverlayAlpha + 1 / 150f);

			if (currentOverlayAlpha >= 1)
			{
				OnFadeOutCompleted.Invoke();
			}
		}
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

	public void FadeOut()
	{
		FadeOverlay.gameObject.SetActive(true);
		isFadingOut = true;
	}
}
