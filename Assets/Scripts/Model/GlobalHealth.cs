using System.Xml.Schema;
using System;
using UnityEngine;

public class GlobalHealth
{
	private float totalHealth;
	private float remainingHealth;

	private GlobalHealth()
	{
		totalHealth = 0;
		remainingHealth = 0;
	}

	public static GlobalHealth GetInstance()
	{
		return GlobalHeathHolder.instance;
	}

	public void DecreaseHealth(float amount)
	{
		remainingHealth = Mathf.Max(0, remainingHealth - amount);
	}

	public float TotalHealth { get => totalHealth; set => totalHealth = value; }
	public float RemainingHealth { get => remainingHealth; set => remainingHealth = value; }

	internal static class GlobalHeathHolder
	{
		internal static GlobalHealth instance = new GlobalHealth();
	}
}