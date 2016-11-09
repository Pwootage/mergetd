using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Wave {
	public EnemyStats GIStats;
	public EnemyStats BIStats;
	public EnemyStats RIStats;

	public EnemyStats GMStats;
	public EnemyStats BMStats;
	public EnemyStats RMStats;

	public EnemyStats GTStats;
	public EnemyStats BTStats;
	public EnemyStats RTStats;

	public int spawnCount;
	public float timeBetweenSpawns = 1;
}

[Serializable]
public struct EnemyTypes {
	public GameObject greenInfantry;
	public GameObject blueInfantry;
	public GameObject redInfantry;

	public GameObject greenMech;
	public GameObject blueMech;
	public GameObject redMech;

	public GameObject greenTank;
	public GameObject blueTank;
	public GameObject redTank;
}

public class WaveController : MonoBehaviour {
	public EnemyTypes enemyTypes;
	public GameObject playerBase;
	public float timeBetweenWaves = 5;
	public List<Wave> waves = new List<Wave>();
	[HideInInspector]
	public List<Vector2> path = new List<Vector2>();
	private int currentWaveNumber = 0;
	private int currentSpawnNumber = 0;
	private float timeUntilNextSpawn = 0;
	private float timeUntilNextWave = 0;
	private GameState state;

	void Start() {
		state = GameState.FindInScene();
		timeUntilNextWave = timeBetweenWaves * 2;
	}

	void Update() {
		//Done with all waves
		if (currentWaveNumber >= waves.Count) {
			return;
		}

		Wave currentWave = waves[currentWaveNumber];

		// Wait for next wave
		if (timeUntilNextWave >= 0) {
			timeUntilNextWave -= Time.deltaTime;
			UpdateTimerUI();
			return;
		}

		// Check to see if wave is done
		if (currentSpawnNumber >= currentWave.spawnCount) {
			currentWaveNumber++;
			if (currentWaveNumber >= waves.Count) {
				state.NotifySpawningIsDone();
				return;
			}
			currentSpawnNumber = 0;
			Wave nextWave = waves[currentWaveNumber];
			timeUntilNextWave = timeBetweenWaves;
			timeUntilNextSpawn = nextWave.timeBetweenSpawns;
			return;
		}

		// Wait for spawn
		if (timeUntilNextSpawn >= 0) {
			timeUntilNextSpawn -= Time.deltaTime;
			UpdateTimerUI();
			return;
		}

		//Actual spawn
		int randEnemy = currentWaveNumber;

		Vector2 look = path[0] - path[1];
		float rotation = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg + 90f;

		GameObject enemy;
		EnemyAI ai;
		if (randEnemy == 0) {
			enemy = GameObject.Instantiate(enemyTypes.greenInfantry);
			ai = enemy.GetComponent<GreenInfantryAI>();
			ai.stats = currentWave.GIStats;
		} else if (randEnemy == 1) {
			enemy = GameObject.Instantiate(enemyTypes.blueInfantry);
			ai = enemy.GetComponent<BlueInfantryAI>();
			ai.stats = currentWave.BIStats;
		} else if (randEnemy == 2) {
			enemy = GameObject.Instantiate(enemyTypes.redInfantry);
			ai = enemy.GetComponent<RedInfantryAI>();
			ai.stats = currentWave.RIStats;
		} else if (randEnemy == 3) {
			enemy = GameObject.Instantiate(enemyTypes.greenMech);
			ai = enemy.GetComponent<GreenMechAI>();
			ai.stats = currentWave.GMStats;

		} else if (randEnemy == 4) {
			enemy = GameObject.Instantiate(enemyTypes.blueMech);
			ai = enemy.GetComponent<BlueMechAI>();
			ai.stats = currentWave.BMStats;
		} else if (randEnemy == 5) {
			enemy = GameObject.Instantiate(enemyTypes.redMech);
			ai = enemy.GetComponent<RedMechAI>();
			ai.stats = currentWave.RMStats;
		} else if (randEnemy == 6) {
			enemy = GameObject.Instantiate(enemyTypes.greenTank);
			ai = enemy.GetComponent<GreenTankAI>();
			ai.stats = currentWave.GTStats;
		} else if (randEnemy == 7) {
			enemy = GameObject.Instantiate(enemyTypes.blueTank);
			ai = enemy.GetComponent<BlueTankAI>();
			ai.stats = currentWave.BTStats;
		} else {
			enemy = GameObject.Instantiate(enemyTypes.redTank);
			ai = enemy.GetComponent<RedTankAI>();
			ai.stats = currentWave.RTStats;
		}
		enemy.transform.position = gameObject.transform.position;
		foreach (Vector2 waypoint in path) {
			ai.path.Enqueue(waypoint);
		}
		ai.rotation = rotation;

		// Next spawn
		timeUntilNextSpawn += currentWave.timeBetweenSpawns;
		UpdateTimerUI();
		currentSpawnNumber++;
	}

	void UpdateTimerUI() {
		if (timeUntilNextWave >= 0) {
			state.GetUIController().UpdateTimeUntilWave(timeUntilNextWave);
		} else if (currentWaveNumber < waves.Count) {
			Wave currentWave = waves[currentWaveNumber];
			state.GetUIController().UpdateSpawnsRemaining(currentWave.spawnCount - currentSpawnNumber);
		}
	}
}
