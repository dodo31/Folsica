using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHeadHumanBasicalController : TowerHeadController
{
	public Vector3 TargetDirection(float range)
	{
		Vector3 hostTowerPosition = this.HostTowerPosition();

		EnemyController[] enemies = GameObject.FindObjectsOfType<EnemyController>();
		EnemyController selectedEnemy = null;

		float minDistanceFromCenter = float.PositiveInfinity;

		for (int i = 0; i < enemies.Length; i++)
		{
			EnemyController enemy = enemies[i];
			Vector3 enemyPosition = enemy.transform.position;

			float distanceFromTower = Vector3.Distance(enemyPosition, hostTowerPosition);
			float distanceFromCenter = Vector3.Distance(enemy.transform.position, Vector3.zero);

			if (distanceFromTower < range)
			{
				if (distanceFromCenter < minDistanceFromCenter)
				{
					selectedEnemy = enemy;
					minDistanceFromCenter = distanceFromCenter;
				}
			}
		}

		if (selectedEnemy != null)
		{
			return (selectedEnemy.transform.position - hostTowerPosition).normalized;
		}
		else
		{
            return Vector3.positiveInfinity;
		}
	}
}
