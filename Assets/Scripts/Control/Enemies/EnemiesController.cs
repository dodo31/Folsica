using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
	private const string ENEMIES_PATH = "Models/Enemies/";

	public Transform PlayArea;

	public PathController[] Paths;

	private List<EnemyController> enemies;

	protected void Start()
	{
		enemies = new List<EnemyController>();
	}

	private void LateUpdate()
	{
		this.SpawnRandomEnemy();
	}

	public void SpawnRandomEnemy()
	{
		float randomNumber = UnityEngine.Random.Range(0, 4);

		if (randomNumber < 1)
		{
			this.SpawRobotClassic();
		}
		else if (randomNumber >= 1 && randomNumber < 2)
		{
			this.SpawRobotHorse();
		}
		else if (randomNumber >= 2 && randomNumber < 3)
		{
			this.SpawRobotHealer();
		}
		else if (randomNumber >= 3 && randomNumber < 4)
		{
			this.SpawRobotCar();
		}
	}

	public void SpawRobotClassic()
	{
		GameObject robotClassicObject = Resources.Load<GameObject>(ENEMIES_PATH + "Robot Classic");
		this.SpwanEnemy(robotClassicObject);
	}

	public void SpawRobotHorse()
	{
		GameObject robotHorseObject = Resources.Load<GameObject>(ENEMIES_PATH + "Robot Horse");
		this.SpwanEnemy(robotHorseObject);
	}

	public void SpawRobotHealer()
	{
		GameObject robotHealerObject = Resources.Load<GameObject>(ENEMIES_PATH + "Robot Healer");
		this.SpwanEnemy(robotHealerObject);
	}

	public void SpawRobotCar()
	{
		GameObject robotCarObject = Resources.Load<GameObject>(ENEMIES_PATH + "Robot Car");
		this.SpwanEnemy(robotCarObject);
	}

	public void SpwanEnemy(GameObject enemyPrefab)
	{
        GameObject enemyObject = Instantiate<GameObject>(enemyPrefab);
		enemyObject.transform.localScale = Vector3.one;
		enemyObject.transform.SetParent(PlayArea.transform);

		EnemyController newEnemy = enemyObject.GetComponent<EnemyController>();
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