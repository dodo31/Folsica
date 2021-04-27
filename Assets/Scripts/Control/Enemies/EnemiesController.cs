using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
	private const string ENEMIES_PATH = "Models/Enemies/";

	public Transform PlayArea;

	public PathController[] Paths;

	private List<EnemyController> enemies;

	private int frameIndex;

	private Waves waves;

	private List<int> frames;

	protected void Start()
	{
		enemies = new List<EnemyController>();

		waves = new Waves();
		frameIndex = 0;

		frames = new List<int>(waves.EnemyTimes);
	}

	private void FixedUpdate()
	{
		int frameIndexIndex = frames.IndexOf(frameIndex);

		if (frameIndexIndex >= 0)
		{
			int enemyId = waves.EnemyPaths[frameIndexIndex];

			switch (enemyId)
			{
			case 0:
				this.SpawRobotClassic();
				break;
			case 1:
				this.SpawRobotHorse();
				break;
			case 2:
				this.SpawRobotHealer();
				break;
			case 3:
				this.SpawRobotCar();
				break;
			case 4:
				this.SpawRobotBoss();
				break;
			}
		}

		frameIndex++;
	}

	public void SpawnRandomEnemy()
	{
		float randomNumber = UnityEngine.Random.Range(0, 5);

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
		else if (randomNumber >= 4 && randomNumber < 5)
		{
			this.SpawRobotBoss();
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

	public void SpawRobotBoss()
	{
		GameObject robotBossObject = Resources.Load<GameObject>(ENEMIES_PATH + "Robot Boss");
		this.SpwanEnemy(robotBossObject);
	}

	public void SpwanEnemy(GameObject enemyPrefab)
	{
		GameObject newEnemyObject = Instantiate<GameObject>(enemyPrefab);
		newEnemyObject.tag = "Enemy";
		newEnemyObject.transform.localScale = Vector3.one;
		newEnemyObject.transform.SetParent(PlayArea.transform);

		EnemyController newEnemy = newEnemyObject.GetComponent<EnemyController>();
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