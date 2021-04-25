using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
	public Transform Environement;

	public PathController[] Paths;

	private List<EnemyController> enemies;

	protected void Start()
	{
		enemies = new List<EnemyController>();
		this.SpwanEnemy();
	}

	private void LateUpdate()
	{
        this.SpwanEnemy();
	}

	public void SpwanEnemy()
	{
		GameObject newEnemyObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		newEnemyObject.transform.localScale = Vector3.one * 0.05f;
		newEnemyObject.transform.SetParent(Environement.transform);

		EnemyController newEnemy = newEnemyObject.AddComponent<EnemyController>();
		enemies.Add(newEnemy);

		PathController path = this.SelectRandomPath();
		StepPoint[] stepPoints = path.ComputeStepPoints(newEnemy);

		newEnemy.StartTraveling(stepPoints);
	}

	private PathController SelectRandomPath()
	{
		int randomIndex = UnityEngine.Random.Range(0, Paths.Length);
		return Paths[randomIndex];
	}
}