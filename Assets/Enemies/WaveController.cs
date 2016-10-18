using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Wave {
	public EnemyStats stats;
	public int spawnCount;
	public float timeBetweenSpawns = 1;
}

public class WaveController : MonoBehaviour {
	public GameObject enemyType;
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
		timeUntilNextWave = timeBetweenWaves * 2;;
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
		if (timeUntilNextSpawn >=  0) {
			timeUntilNextSpawn -= Time.deltaTime;
			UpdateTimerUI();
			return;
		}

		//Actual spawn
		GameObject enemy = GameObject.Instantiate(enemyType);
		enemy.transform.position = gameObject.transform.position;
		EnemyAI ai = enemy.GetComponent<EnemyAI>();
		foreach (Vector2 waypoint in path) {
			ai.path.Enqueue(waypoint);
		}
		ai.stats = currentWave.stats;

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