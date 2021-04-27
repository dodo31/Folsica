using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
	private const string ENEMIES_PATH = "Models/Enemies/";

	public Transform PlayArea;

	public PathController[] Paths;

	private List<EnemyController> enemies;

	public event Action<int> OnNewWave;
	public event Action<EnemyController> OnEnemyReachedDigger;

	private int frameIndex;

	private List<int> waveTimes;
	private List<int> enemyTimes;

	private List<int> enemyPaths;

	private int curentDay;

	protected void Start()
	{
		enemies = new List<EnemyController>();

		Waves waves = new Waves();
		frameIndex = 0;

		waveTimes = new List<int>(waves.WaveTimes);
		enemyTimes = new List<int>(waves.EnemyTimes);
		enemyPaths = new List<int>(waves.EnemyPaths);

		curentDay = 1;
	}

	private void FixedUpdate()
	{
		int enemyTimeIndex = enemyTimes.IndexOf(frameIndex);

		if (enemyTimeIndex >= 0)
		{
			int enemyId = enemyPaths[enemyTimeIndex];

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

		int waveTimeIndex = waveTimes.IndexOf(frameIndex);

		if (waveTimeIndex >= 0)
		{
			OnNewWave.Invoke(curentDay);
			curentDay++;
		}

		frameIndex++;
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
		newEnemy.OnReachedTarget += OnEnemyReachedDigger.Invoke;

		PathController path = Paths[0];
		StepPoint[] stepPoints = path.ComputeStepPoints(newEnemy);

		newEnemy.StartTraveling(stepPoints);
	}
}