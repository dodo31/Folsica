using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHeadAlienBasicalController : TowerHeadController
{
	public override Vector3 SelectTargetDirection(float range)
	{
		return Vector3.positiveInfinity;
	}
}
