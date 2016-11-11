using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerClickable : MonoBehaviour {
    private GameState state;

    public Text text;

    [HideInInspector] public TowerBuilder builder;

    public int id {
        get { return _id; }
        set {
            _id = value;
            UpdateText();
        }
    }

    [SerializeField]
    private int _id;

    void Start() {
        Debug.Log("Adding click listener");
        state = GameState.FindInScene();
        Button b = GetComponent<Button>();
        b.onClick.AddListener(this.OnClick);
        UpdateText();
    }

    void Update() {
    }

    public void OnClick() {
        builder.SelectTower(id);
		state.getAudioPlayer().playSelectBlip();
    }

    public void UpdateText() {
        if (state == null) {
            state = GameState.FindInScene();
        }
        text.text = state.towers[id].GetComponent<TowerAI>().stats.description();
    }
}
