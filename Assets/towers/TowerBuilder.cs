using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerBuilder : MonoBehaviour {
	//TODO: this needs to be a list
	public GameObject tower;
	public Text moneyLabel;
	public int cost = 10;
	public int starterMoney = 20;
	int money;

	void Start() {
		UpdateMoneyLabel();
		money = starterMoney;
	}

	void Update() {
	
	}

	void UpdateMoneyLabel() {
		moneyLabel.text = "Money: " + money;
	}

	void OnMouseDown() {
		if (money >= cost) {
			Vector3 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int x = Mathf.RoundToInt(loc.x);
			int y = Mathf.RoundToInt(loc.y);
			money -= cost;
			GameObject newTower = GameObject.Instantiate(tower);
			newTower.transform.position = new Vector3(x, y);
			UpdateMoneyLabel();
		}
	}

	public void AddMoney(int money) {
		this.money += money;
		UpdateMoneyLabel();
	}
}
