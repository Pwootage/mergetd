using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PickerController : MonoBehaviour {
	public Text descriptionText;
	private GameState state;
	private GameObject[] pickerObjects;
	private TowerStatModifier modifier;
    private Boolean needsUpdate = false;

	public static PickerController FindInScene() {
		return GameObject.Find("PickerUI").GetComponent<PickerController>();
	}

	void Start() {
		state = GameState.FindInScene();
		pickerObjects = new GameObject[state.towerCount];
		GameObject prototype = transform.Find("TowerPicker").gameObject;
		pickerObjects[0] = prototype;

		for (int i = 1; i < state.towerCount; i++) {
			var newPicker = GameObject.Instantiate(prototype);
			pickerObjects[i] = newPicker;


			RectTransform rectTransform = newPicker.GetComponent<RectTransform>();
			rectTransform.SetParent(prototype.transform.parent, false);

			newPicker.transform.localPosition = prototype.transform.localPosition + new Vector3(i * 300, 0);
		}

		for (int i = 0; i < state.towerCount; i++) {
			var picker = pickerObjects[i];
			Button b = picker.transform.FindChild("Button").gameObject.GetComponent<Button>();
			int toMerge = i;
			b.onClick.AddListener(() => {
				merge(toMerge);
			});
		}
	}

	void Update() {
        if (state == null) {
            state = GameState.FindInScene();
        }
	    if (!needsUpdate) {
	        return;
	    }
	    needsUpdate = false;
        //Update the text(s)
        this.descriptionText.text = modifier.getDescription();
        for (int i = 0; i < state.towerCount; i++) {
            Text textUI = pickerObjects[i].transform.Find("Stats").gameObject.GetComponent<Text>();
            TowerAI tower = state.towers[i].GetComponent<TowerAI>();
            TowerStats stats = modifier.applyModifier(tower.getFinalStats());

            string text = stats.description() +
                "\nCost:" + stats.getCost() +
                "\nAttack Rate: " + stats.getRateOfFire() +
                "\nDamage: " + stats.getDamage() +
                "\nRange: " + stats.getRange();

            foreach (EnemyEffect effect in stats.getEffects()) {
                text += "\nSlow: " + ((1 - effect.slowMultiplier) * 100) + "%" +
                    "\nDamage Per Second: " + effect.damagePerSecond +
                    "\nDuration: " + effect.duration + "s";
            }

            textUI.text = text;
        }
    }


	public void merge(int id) {
		state.towers[id].GetComponent<TowerAI>().statModifiers.Add(modifier);
		state.GetUIController().hidePicker();
	}

	public void setModifier(TowerStatModifier modifier) {
		this.modifier = modifier;
	    needsUpdate = true;
	}
}
