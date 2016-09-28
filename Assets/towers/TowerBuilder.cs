using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerBuilder : MonoBehaviour {
	//TODO: this needs to be a list
	public GameObject tower;
	public int cost = 10;
	private GameState state;

	void Start() {
		state = GameState.FindInScene();
	}

	void Update() {
	
	}

	void OnMouseDown() {
		if (state.getMoney() >= cost) {
			state.spendMoney(cost);

			Vector3 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int x = Mathf.RoundToInt(loc.x);
			int y = Mathf.RoundToInt(loc.y);
			GameObject newTower = GameObject.Instantiate(tower);
			newTower.transform.position = new Vector3(x, y);
		}
	}
}
