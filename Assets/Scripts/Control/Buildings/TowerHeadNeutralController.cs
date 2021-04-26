using UnityEngine;

public class TowerHeadNeutralController : TowerHeadController
{
	public override Vector3 SelectTargetDirection(float range)
	{
		return Vector3.positiveInfinity;
	}
}