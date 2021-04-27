using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyController : MonoBehaviour
{
	public event Action<float, Race> OnTransaction;

	private Economy economy;

	private void Start()
	{
		economy = Economy.GetInstance();
	}

	public void GainResource(float amount, Race resourceType)
	{
		economy.GainResource(amount, resourceType);
		OnTransaction.Invoke(amount, resourceType);
	}

	public bool SpendResource(float amount, Race resourceType)
	{
		bool hasBeenSpent = economy.SpendResource(amount, resourceType);

		if (hasBeenSpent)
		{
			OnTransaction.Invoke(-amount, resourceType);
			return true;
		}
		else
		{
			return false;
		}
	}
}
