using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TowerBuilder : MonoBehaviour {
    [HideInInspector]
	public GameObject tower;

    public GameObject uiObject;
    public GameObject towerChoiceClickablePrototype;
	private GameState state;
    private List<GameObject> towerChoiceClickables = new List<GameObject>();

	void Start() {
		state = GameState.FindInScene();
	    for (int i = 0; i < state.towerCount; i++) {
	        GameObject obj = Instantiate(towerChoiceClickablePrototype);
	        TowerClickable clickable = obj.GetComponent<TowerClickable>();
	        clickable.builder = this;
	        clickable.id = i;

	        RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.SetParent(uiObject.transform);
            rectTransform.anchoredPosition = new Vector3(-15, i * 35);
	        towerChoiceClickables.Add(obj);
	    }
        SelectTower(0);
	}

	void Update() {
	
	}

	void OnMouseDown() {
	    TowerAI tower = this.tower.GetComponent<TowerAI>();
		if (state.getMoney() >= tower.stats.cost) {
			state.spendMoney(tower.stats.cost);

			Vector3 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int x = Mathf.RoundToInt(loc.x);
			int y = Mathf.RoundToInt(loc.y);

			if (!state.map.isBuildable(x, y)) {
				return;
			}

			GameObject oldTower = state.map.getTower(x, y);
			if (oldTower != null) {
				TowerAI oldTowerAI = oldTower.GetComponent<TowerAI>();
				state.GiveMoney(oldTowerAI.stats.cost / 2);
				Destroy(oldTower);
			}

			GameObject newTower = GameObject.Instantiate(this.tower);
			newTower.transform.position = new Vector3(x, y);

			state.map.setTower(x, y, newTower);
		}
	}

    public void SelectTower(int id) {
        foreach (GameObject obj in towerChoiceClickables) {
            obj.GetComponent<TowerClickable>().UpdateText();
        }
        tower = state.towers[id];
        TowerStats towerStats = tower.GetComponent<TowerAI>().stats;
        state.GetUIController().updateStatView(id, towerStats);
    }
}
