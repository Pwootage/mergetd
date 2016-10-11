using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveController : MonoBehaviour {
	public GameObject enemyType;
	public GameObject playerBase;
	public int spawnCount = 10;
	public float timePerSpawn = 1;
	public float timeBeforeWave = 20;
	private float timer;
	public GameState state;
	public List<Vector2> path = new List<Vector2>();

	void Start() {
		state = GameState.FindInScene();
		timer = timePerSpawn;
	}

	void Update() {
		if (timeBeforeWave >= 0) {
			timeBeforeWave -= Time.deltaTime;
		} else if (spawnCount <= 0) {
			state.NotifySpawningIsDone();
		} else {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				UpdateTimerUI();
				spawnCount -= 1;
				timer += timePerSpawn;
				GameObject enemy = GameObject.Instantiate(enemyType);
				enemy.transform.position = gameObject.transform.position;
				EnemyAI ai = enemy.GetComponent<EnemyAI>();
				foreach (Vector2 waypoint in path) {
					ai.path.Enqueue(waypoint);
				}
			}
		}
		UpdateTimerUI();
	}

	void UpdateTimerUI() {
		if (timeBeforeWave >= 0) {
			state.GetUIController().UpdateTimeUntilWave(timeBeforeWave);
		} else {
			state.GetUIController().UpdateSpawnsRemaining(spawnCount);
		}
	}
}