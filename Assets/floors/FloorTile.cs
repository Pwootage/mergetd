using UnityEngine;
using System.Collections;

public class FloorTile : MonoBehaviour {
	void Start() {
		
	}

	void Update() {
	
	}

	public void SetSprite(Sprite sprite) {
		GetComponent<SpriteRenderer>().sprite = sprite;
	}
}
