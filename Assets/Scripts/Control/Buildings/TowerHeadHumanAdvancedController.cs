using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHeadHumanAdvancedController : TowerHeadController
{
	public override Vector3 SelectTargetDirection(float range)
	{
		Vector3 hostTowerPosition = this.HostTowerPosition();

		EnemyController[] enemies = GameObject.FindObjectsOfType<EnemyController>();
		EnemyController selectedEnemy = null;

		float maxEnemyHealth = float.NegativeInfinity;

		for (int i = 0; i < enemies.Length; i++)
		{
			EnemyController enemy = enemies[i];
			Vector3 enemyPosition = enemy.transform.position;
			float enemyHealth = enemy.Health;

			float distanceFromTower = Vector3.Distance(enemyPosition, hostTowerPosition);

			if (distanceFromTower < range)
			{
				if (enemyHealth > maxEnemyHealth)
				{
					selectedEnemy = enemy;
					maxEnemyHealth = enemyHealth;
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
