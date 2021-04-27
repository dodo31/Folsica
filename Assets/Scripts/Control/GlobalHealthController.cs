using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHealthController : MonoBehaviour
{
	private float totalHealth = 10000;

	public event Action<float> OnHealthDecreased;

	private GlobalHealth globalHealth;

	protected void Awake()
	{
		globalHealth = GlobalHealth.GetInstance();
		globalHealth.TotalHealth = totalHealth;
        globalHealth.RemainingHealth = totalHealth;
	}

	public void DecreaseHealth(float amount)
	{
        globalHealth.DecreaseHealth(amount);
        OnHealthDecreased.Invoke(amount);
	}
}
