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
	private AudioPlayer audioPlayer;
	private bool spawning;


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
		spawning = true;
		audioPlayer = GetComponent<AudioPlayer>();
	}

	void Update() {
		if (!spawning && GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
			uiController.ShowWinUI("You win!");
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
}
