using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveController : MonoBehaviour {
	public GameObject enemyType;
	public GameObject playerBase;
	public Text winUI;
	public Text waveUI;
	public int spawnCount = 10;
	public float timePerSpawn = 1;
	public float timeBeforeWave = 20;
	private float timer;

	void Start() {
		timer = timePerSpawn;
	}

	void Update() {
		if (timeBeforeWave >= 0) {
			timeBeforeWave -= Time.deltaTime;
			UpdateTimerUI();
		} else if (spawnCount <= 0) {
			if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
				winUI.text = "You win!";
				winUI.gameObject.SetActive(true);
			}
		} else {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				UpdateTimerUI();
				spawnCount -= 1;
				timer += timePerSpawn;
				GameObject enemy = GameObject.Instantiate(enemyType);
				enemy.transform.position = gameObject.transform.position;
				EnemyAI ai = enemy.GetComponent<EnemyAI>();
				ai.path.Enqueue(new Vector2(playerBase.transform.position.x, gameObject.transform.position.x));
				ai.path.Enqueue(playerBase.transform.position);
			}
		}
	}

	void UpdateTimerUI() {
		if (timeBeforeWave >= 0) {
			waveUI.text = "Time until next wave: " + Mathf.FloorToInt(timeBeforeWave) + "s";
		} else {
			waveUI.text = "Spawns Remaining: " + spawnCount;
		}
	}
}