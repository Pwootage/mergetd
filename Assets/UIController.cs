using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public GameState state;
    public Text livesUI;
    public Text waveUI;
    public Text moneyUI;
    public Text gameOverUI;
    public Text towerStatUI;
	public Button nextWaveButton;
	public GameObject PickerUI;

    void Start() {
		state = GameState.FindInScene();
    }

    void Update() {
    }

	public void showNextWaveButton() {
		nextWaveButton.gameObject.SetActive(true);
	}

	public void hideNextWaveButton() {
		nextWaveButton.gameObject.SetActive(false);
	}

    public void hideWinText() {
        gameOverUI.gameObject.SetActive(false);
    }

    public void UpdateMoneyLabel(float money) {
		moneyUI.text = "Money: " + (int)money;
    }

    public void ShowWinUI(string text) {
        gameOverUI.text = text;
        gameOverUI.gameObject.SetActive(true);
    }

    public void UpdateTimeUntilWave(float timeBeforeWave) {
		if (float.IsInfinity(timeBeforeWave)) {
			waveUI.text = "Click 'Next Wave' to start next wave";
		} else {
			waveUI.text = "Time until next wave: " + Mathf.FloorToInt(timeBeforeWave) + "s";
		}
        
    }

    public void UpdateSpawnsRemaining(int spawnCount) {
        waveUI.text = "Spawns Remaining: " + spawnCount;
    }

    public void UpdateLivesUI(int lives, int startLives) {
        livesUI.text = "Lives: " + lives + "/" + startLives;
    }

    public void updateStatView(int index, TowerStats stats) {
		string text = stats.description() +
		              "\nCost:" + stats.getCost() +
		              "\nAttack Rate: " + stats.getRateOfFire() +
		              "\nDamage: " + stats.getDamage() +
		              "\nRange: " + stats.getRange();

		foreach (EnemyEffect effect in stats.getEffects()) {
			text += "\nSlow: " + ((1-effect.slowMultiplier) * 100) + "%" +
				"\nDamage Per Second: " + effect.damagePerSecond +
				"\nDuration: " + effect.duration + "s";
		}

        towerStatUI.text = text;
    }

	public void showPicker() {
		if (state == null) {
			state = GameState.FindInScene();
		}
		state.setSpawningPaused(true);
		PickerUI.SetActive(true);
	}

	public void hidePicker() {
		if (state == null) {
			state = GameState.FindInScene();
		}
		state.setSpawningPaused(false);
		PickerUI.SetActive(false);
	}
}
