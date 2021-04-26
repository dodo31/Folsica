using UnityEngine;

public abstract class TowerHeadController : TowerStageController
{
	public float Cadence = 0;

	public float PowerMultiplicator = 1;

	public abstract Vector3 SelectTargetDirection(float range);
}