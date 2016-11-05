using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Wave {
    public GreenInfantryStats GIStats;
	public BlueInfantryStats BIStats;
    public RedInfantryStats RIStats;
    public GreenMechStats GMStats;
    public BlueMechStats BMStats;
    public RedMechStats RMStats;
    public GreenTankStats GTStats;
    public BlueTankStats BTStats;
    public RedTankStats RTStats;
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
        int randEnemy = currentWaveNumber;

		//GameObject enemy = GameObject.Instantiate(enemyType);
		//enemy.transform.position = gameObject.transform.position;

        if (randEnemy == 0) {
            enemyType = GameObject.FindGameObjectWithTag("Green Infantry");

            GameObject enemy = GameObject.Instantiate(enemyType);
            enemy.transform.position = gameObject.transform.position;

            GreenInfantryAI ai = enemy.GetComponent<GreenInfantryAI>();
            foreach (Vector2 waypoint in path) {
                ai.path.Enqueue(waypoint);
            }
            ai.stats = currentWave.GIStats;
        }
        else if (randEnemy == 1) {
            enemyType = GameObject.FindGameObjectWithTag("Blue Infantry");

            GameObject enemy = GameObject.Instantiate(enemyType);
            enemy.transform.position = gameObject.transform.position;

            BlueInfantryAI ai = enemy.GetComponent<BlueInfantryAI>();
            foreach (Vector2 waypoint in path) {
                ai.path.Enqueue(waypoint);
            }
            ai.stats = currentWave.BIStats;
        }
        else if (randEnemy == 2) {
            enemyType = GameObject.FindGameObjectWithTag("Red Infantry");

            GameObject enemy = GameObject.Instantiate(enemyType);
            enemy.transform.position = gameObject.transform.position;

            RedInfantryAI ai = enemy.GetComponent<RedInfantryAI>();
            foreach (Vector2 waypoint in path) {
                ai.path.Enqueue(waypoint);
            }
            ai.stats = currentWave.RIStats;
        }
        else if (randEnemy == 3) {
            enemyType = GameObject.FindGameObjectWithTag("Green Mech");

            GameObject enemy = GameObject.Instantiate(enemyType);
            enemy.transform.position = gameObject.transform.position;

            GreenMechAI ai = enemy.GetComponent<GreenMechAI>();
            foreach (Vector2 waypoint in path) {
                ai.path.Enqueue(waypoint);
            }
            ai.stats = currentWave.GMStats;

        }
        else if (randEnemy == 4) {
            enemyType = GameObject.FindGameObjectWithTag("Blue Mech");

            GameObject enemy = GameObject.Instantiate(enemyType);
            enemy.transform.position = gameObject.transform.position;

            BlueMechAI ai = enemy.GetComponent<BlueMechAI>();
            foreach (Vector2 waypoint in path) {
                ai.path.Enqueue(waypoint);
            }
            ai.stats = currentWave.BMStats;
        }
        else if (randEnemy == 5) {
            enemyType = GameObject.FindGameObjectWithTag("Red Mech");

            GameObject enemy = GameObject.Instantiate(enemyType);
            enemy.transform.position = gameObject.transform.position;

            RedMechAI ai = enemy.GetComponent<RedMechAI>();
            foreach (Vector2 waypoint in path) {
                ai.path.Enqueue(waypoint);
            }
            ai.stats = currentWave.RMStats;
        }
        else if (randEnemy == 6) {
            enemyType = GameObject.FindGameObjectWithTag("Green Tank");

            GameObject enemy = GameObject.Instantiate(enemyType);
            enemy.transform.position = gameObject.transform.position;

            GreenTankAI ai = enemy.GetComponent<GreenTankAI>();
            foreach (Vector2 waypoint in path) {
                ai.path.Enqueue(waypoint);
            }
            ai.stats = currentWave.GTStats;
        }
        else if (randEnemy == 7) {
            enemyType = GameObject.FindGameObjectWithTag("Blue Tank");

            GameObject enemy = GameObject.Instantiate(enemyType);
            enemy.transform.position = gameObject.transform.position;

            BlueTankAI ai = enemy.GetComponent<BlueTankAI>();
            foreach (Vector2 waypoint in path) {
                ai.path.Enqueue(waypoint);
            }
            ai.stats = currentWave.BTStats;
        }
        else if (randEnemy == 8) {
            enemyType = GameObject.FindGameObjectWithTag("Red Tank");

            GameObject enemy = GameObject.Instantiate(enemyType);
            enemy.transform.position = gameObject.transform.position;

            RedTankAI ai = enemy.GetComponent<RedTankAI>();
            foreach (Vector2 waypoint in path) {
                ai.path.Enqueue(waypoint);
            }
            ai.stats = currentWave.RTStats;
        }

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
