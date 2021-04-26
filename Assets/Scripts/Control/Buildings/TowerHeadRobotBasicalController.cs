using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHeadRobotBasicalController : TowerHeadController
{
	private float lastTargetAngle;

	private void Start()
	{
		lastTargetAngle = 0;
	}

	public override Vector3 SelectTargetDirection(float range)
	{
		float currentTargetAngle = lastTargetAngle - Mathf.PI / 4f;
		lastTargetAngle = currentTargetAngle;

		float directionX = Mathf.Cos(currentTargetAngle);
		float directionY = Mathf.Sin(currentTargetAngle);

		return new Vector3(directionX, 0, directionY);
	}
}
