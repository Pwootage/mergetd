using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {
    public readonly int towerCount = 5;
	public int starterMoney = 20;
	public int startLives = 10;
    public List<GameObject> towers = new List<GameObject>();
    public GameMap map;
    private int lives;
	private float money;
	private UIController uiController;
	private PickerController pickerController;
	private AudioPlayer audioPlayer;
	private bool spawning;
	private bool showModifierUI = false;
	private bool pauseSpawning = false;
	public float normalSpeed = 1;
	public float fastSpeed = 3;


	public static GameState FindInScene() {
		return GameObject.Find("GameController").GetComponent<GameState>();
	}

	void Start() {
		money = starterMoney;
		lives = startLives;
		uiController = GameObject.Find("UI").GetComponent<UIController>();
		uiController.hideWinText();
		uiController.UpdateLivesUI(lives, startLives);
		uiController.UpdateMoneyLabel(money);
		pickerController = PickerController.FindInScene();
		uiController.hidePicker();
		spawning = true;
		audioPlayer = GetComponent<AudioPlayer>();
		//spawnModifier(1);
	}

	void Update() {
		if (showModifierUI /* && GameObject.FindGameObjectsWithTag("Enemy").Length == 0*/ ) {
			uiController.showNextWaveButton();
			uiController.showPicker();
			showModifierUI = false;
		}
		if (!spawning && GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
			uiController.ShowWinUI("You win!");
		}
		if (Input.GetKey("f")) {
			Time.timeScale = fastSpeed;
		} else {
			Time.timeScale = normalSpeed;
		}
	}

	public UIController GetUIController() {
		return uiController;
	}

	public AudioPlayer getAudioPlayer() {
		return audioPlayer;
	}

	public float getMoney() {
		return money;
	}

	public void GiveMoney(float moneyValue) {
		this.money += moneyValue;
		uiController.UpdateMoneyLabel(this.money);
	}

	/**
	 * Does not currently check to make sure you have enough money. Make sure you do this first!
	 */
	public void spendMoney(float money) {
		this.money -= money;
		uiController.UpdateMoneyLabel(this.money);
	}

	public void NotifySpawningIsDone() {
		spawning = false;
	}

	public void TakeDamage(int amount) {
		lives -= amount;
		if (lives < 0) {
			lives = 0;
		}
		uiController.UpdateLivesUI(lives, startLives);
		if (lives <= 0) {
			uiController.ShowWinUI("You died!");
			GameObject.Find("base").SetActive(false);
		}
	}

	public void setSpawningPaused(bool paused) {
		pauseSpawning = paused;
	}

	public bool isSpawningPaused() {
		return pauseSpawning;
	}

	public void spawnModifier(int wave) {
		TowerStatModifier modifier = null;
		var type = Random.Range(0, 4);
		float strength = Random.Range(0.7f, 1.3f);
		float waveMod = ((float)wave + 1f) * 0.5f;
		switch (type) {
			case 0:
				modifier = new PercentAttackDamageBoost(strength * waveMod * 0.1f);
				break;
			case 1:
				modifier = new PercentAttackSpeedBoost(strength * waveMod * 0.05f);
				break;
			case 2: 
				modifier = new FlatRangeBoost(strength * waveMod * 0.5f);
				break;
			case 3:
				modifier = new CostReduction(Mathf.CeilToInt(strength * waveMod));
				break;
			default:
				Debug.Log("spawning invalid modifier D:");
				break;
		}

		showModifierUI = true;
		pickerController.setModifier(modifier);
	}
}
