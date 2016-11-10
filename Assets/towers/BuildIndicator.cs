using UnityEngine;
using System.Collections;

public class BuildIndicator : MonoBehaviour {
	public Color invisibleColor;
	public Color okColor;
	public Color warnColor;
	public Color badColor;
	private SpriteRenderer spriteRenderer;
	private GameState state;


	void Start () {
		this.state = GameState.FindInScene();
		this.spriteRenderer = GetComponent<SpriteRenderer>();
		this.spriteRenderer.color = invisibleColor;
	}

	void Update () {
		Vector3 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		int x = Mathf.RoundToInt(loc.x);
		int y = Mathf.RoundToInt(loc.y);
		transform.position = new Vector3(x, y, -6);
		int tile = state.map.getTileIndex(x, y);
		if (x < 0 || x >= state.map.width || y < 0 || y >= state.map.height) {
			spriteRenderer.color = invisibleColor;
		} else if (!state.map.isBuildable(x, y)) {
			spriteRenderer.color = badColor;
		} else if (state.map.towers[tile] != null) {
			spriteRenderer.color = warnColor;
		} else {
			spriteRenderer.color = okColor;
		}
			
	}
}
