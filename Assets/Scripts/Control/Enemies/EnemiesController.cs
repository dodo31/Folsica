using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
	public Transform PlayArea;

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
		newEnemyObject.transform.localScale = new Vector3(0.1f, 0.05f, 0.05f);
		newEnemyObject.transform.SetParent(PlayArea.transform);

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