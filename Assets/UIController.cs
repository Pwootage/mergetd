using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Text livesUI;
	public Text waveUI;
	public Text moneyUI;
	public Text gameOverUI;

	void Start() {
	
	}

	void Update() {
	
	}

	public void hideWinText() {
		gameOverUI.gameObject.SetActive(false);
	}

	public void UpdateMoneyLabel(int money) {
		moneyUI.text = "Money: " + money;
	}

	public void ShowWinUI(string text) {
		gameOverUI.text = text;
		gameOverUI.gameObject.SetActive(true);
	}

	public void UpdateTimeUntilWave(float timeBeforeWave) {
		waveUI.text = "Time until next wave: " + Mathf.FloorToInt(timeBeforeWave) + "s";
	}

	public void UpdateSpawnsRemaining(int spawnCount) {
		waveUI.text = "Spawns Remaining: " + spawnCount;
	}

	public void UpdateLivesUI(int lives, int startLives) {
		livesUI.text = "Lives: " + lives + "/" + startLives;
	}
}
