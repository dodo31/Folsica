using System;
using UnityEngine;

public class Economy
{
	private float humanResourceAmout;
	private float alienResourceAmout;
	private float robotResourceAmout;

	private Economy()
	{
		humanResourceAmout = 0;
		alienResourceAmout = 0;
		robotResourceAmout = 0;
	}

	public static Economy GetInstance()
	{
		return EconomyHolder.instance;
	}

	public void GainResource(float amount, Race resourceType)
	{
		switch (resourceType)
		{
		case Race.HUMAN:
			humanResourceAmout += amount;
			break;
		case Race.ALIEN:
			alienResourceAmout += amount;
			break;
		case Race.ROBOT:
			robotResourceAmout += amount;
			break;
		}
	}

	public bool SpendResource(float amount, Race resourceType)
	{
		switch (resourceType)
		{
		case Race.HUMAN:
			if (humanResourceAmout > Mathf.Abs(amount))
			{
				humanResourceAmout = amount;
				return true;
			}
			break;
		case Race.ALIEN:
			if (alienResourceAmout > Mathf.Abs(amount))
			{
				alienResourceAmout = amount;
				return true;
			}
			break;
		case Race.ROBOT:
			if (robotResourceAmout > Mathf.Abs(amount))
			{
				robotResourceAmout = amount;
				return true;
			}
			break;
		}

		return false;
	}

	public float HumanResourceAmout { get => humanResourceAmout; set => humanResourceAmout = value; }
	public float AlienResourceAmout { get => alienResourceAmout; set => alienResourceAmout = value; }
	public float RobotResourceAmout { get => robotResourceAmout; set => robotResourceAmout = value; }

	internal static class EconomyHolder
	{
		internal static Economy instance = new Economy();
	}
}