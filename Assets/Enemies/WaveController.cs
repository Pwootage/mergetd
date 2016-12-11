using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Wave {
	public Wave(int count) {
		this.spawnCount = count;
	}

	public int spawnCount;
}

[Serializable]
public struct EnemyTypes {
	public GameObject greenInfantry;
	public EnemyStats greenInfantryStats;
	public GameObject blueInfantry;
	public EnemyStats blueInfantryStats;
	public GameObject redInfantry;
	public EnemyStats redInfantryStats;

	public GameObject greenMech;
	public EnemyStats greenMechStats;
	public GameObject blueMech;
	public EnemyStats blueMechStats;
	public GameObject redMech;
	public EnemyStats redMechStats;

	public GameObject greenTank;
	public EnemyStats greenTankStats;
	public GameObject blueTank;
	public EnemyStats blueTankStats;
	public GameObject redTank;
	public EnemyStats redTankStats;
}

public class WaveController : MonoBehaviour {
	public EnemyTypes enemyTypes;
	public GameObject playerBase;
	public float timeBetweenWaves = 5;
	public int waveCount = 24;
	private List<Wave> waves = new List<Wave>();
	[HideInInspector]
	public List<Vector2> path = new List<Vector2>();
	private int currentWaveNumber = 0;
	private int currentSpawnNumber = 0;
	private float timeUntilNextSpawn = 0;
	private float timeBetweenSpawns = 0;
	private float timeUntilNextWave = 0;
	private GameState state;

	public void nextWave() {
		timeUntilNextWave = 0;
		state.GetUIController().hideNextWaveButton();
	}

	void Start() {
		for (int i = 0; i < waveCount; i++) {
			if (i % 3 == 0) {
				waves.Add(new Wave(10));
			} else if (i % 3 == 1) {
				waves.Add(new Wave(20));
			} else {
				waves.Add(new Wave(3));
			}
		}

		state = GameState.FindInScene();
//		timeUntilNextWave = timeBetweenWaves * 3;
		timeUntilNextWave = float.PositiveInfinity;
		timeBetweenSpawns = 20f / (float)waves[0].spawnCount;
		state.GetUIController().showNextWaveButton();
	}

	void Update() {
		//Don't progress if paused (i.e. "Pick tower" dialog)
		if (state.isSpawningPaused()) {
			return;
		}

		//Done with all waves
		if (currentWaveNumber >= waves.Count) {
			return;
		}
			
		// Wait for next wave
		if (timeUntilNextWave >= 0) {
			timeUntilNextWave -= Time.deltaTime;
			UpdateTimerUI();
			return;
		}

		Wave currentWave = waves[currentWaveNumber];
		state.GetUIController().hideNextWaveButton();

		// Check to see if wave is done
		if (currentSpawnNumber >= currentWave.spawnCount) {
			state.spawnModifier(currentWaveNumber);
			currentWaveNumber++;
			if (currentWaveNumber >= waves.Count) {
				state.NotifySpawningIsDone();
				return;
			}
			currentSpawnNumber = 0;
			Wave nextWave = waves[currentWaveNumber];
			timeUntilNextWave = timeBetweenWaves;
			timeBetweenSpawns = 20f / (float)nextWave.spawnCount;
			timeUntilNextSpawn = 0;
			return;
		}

		// Wait for spawn
		if (timeUntilNextSpawn >= 0) {
			timeUntilNextSpawn -= Time.deltaTime;
			UpdateTimerUI();
			return;
		}

		//Actual spawn
		int randEnemy = UnityEngine.Random.Range(0, 8);

		Vector2 look = path[0] - path[1];
		float rotation = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg + 90f;

		GameObject enemy;
		EnemyAI ai;
		if (randEnemy == 0) {
			enemy = GameObject.Instantiate(enemyTypes.greenInfantry);
			ai = enemy.GetComponent<GreenInfantryAI>();
			ai.stats = enemyTypes.greenInfantryStats.Clone();
		} else if (randEnemy == 1) {
			enemy = GameObject.Instantiate(enemyTypes.blueInfantry);
			ai = enemy.GetComponent<BlueInfantryAI>();
			ai.stats = enemyTypes.blueInfantryStats.Clone();
		} else if (randEnemy == 2) {
			enemy = GameObject.Instantiate(enemyTypes.redInfantry);
			ai = enemy.GetComponent<RedInfantryAI>();
			ai.stats = enemyTypes.redInfantryStats.Clone();
		} else if (randEnemy == 3) {
			enemy = GameObject.Instantiate(enemyTypes.greenMech);
			ai = enemy.GetComponent<GreenMechAI>();
			ai.stats = enemyTypes.greenMechStats.Clone();
		} else if (randEnemy == 4) {
			enemy = GameObject.Instantiate(enemyTypes.blueMech);
			ai = enemy.GetComponent<BlueMechAI>();
			ai.stats = enemyTypes.blueMechStats.Clone();
		} else if (randEnemy == 5) {
			enemy = GameObject.Instantiate(enemyTypes.redMech);
			ai = enemy.GetComponent<RedMechAI>();
			ai.stats = enemyTypes.redMechStats.Clone();
		} else if (randEnemy == 6) {
			enemy = GameObject.Instantiate(enemyTypes.greenTank);
			ai = enemy.GetComponent<GreenTankAI>();
			ai.stats = enemyTypes.greenTankStats.Clone();
		} else if (randEnemy == 7) {
			enemy = GameObject.Instantiate(enemyTypes.blueTank);
			ai = enemy.GetComponent<BlueTankAI>();
			ai.stats = enemyTypes.blueTankStats.Clone();
		} else {
			enemy = GameObject.Instantiate(enemyTypes.redTank);
			ai = enemy.GetComponent<RedTankAI>();
			ai.stats = enemyTypes.greenTankStats.Clone();
		}
		enemy.transform.position = gameObject.transform.position;
		foreach (Vector2 waypoint in path) {
			ai.path.Enqueue(waypoint);
		}
		ai.rotation = rotation;
		float waveEnemyMultiplier = (10f / (float)currentWave.spawnCount);
		float difficultyFactor = (1f + (float)currentWaveNumber * 0.8f) * waveEnemyMultiplier;
		ai.stats.health *= difficultyFactor;
		ai.stats.value *= waveEnemyMultiplier; //Doesn't scale with wave - towers all cost the same amount
		//ai.stats.value = (int)Math.Floor((float)ai.stats.value * difficultyFactor);

		// Next spawn
		timeUntilNextSpawn += timeBetweenSpawns;
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
