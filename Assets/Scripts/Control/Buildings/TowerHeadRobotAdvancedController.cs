using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHeadRobotAdvancedController : TowerHeadController
{
	public override Vector3 SelectTargetDirection(float range)
	{
		float randomAngle = UnityEngine.Random.Range(0, Mathf.PI * 2);
		float directionX = Mathf.Cos(randomAngle);
		float directionY = Mathf.Sin(randomAngle);
		return new Vector3(directionX, 0, directionY);
	}
}
