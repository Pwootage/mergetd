using UnityEngine;
using System.Collections;

public class FloorGenerator : MonoBehaviour {
	public int width = 10;
	public int height = 10;
	public Sprite[] floorTiles = { };
	public GameObject floorTileObject;
	public GameObject playerBase;
	public GameObject enemySpawner;
	private GameObject[] generatedTiles;

	void Start() {
		//Setup click collider
		BoxCollider2D clickCollider = gameObject.GetComponent<BoxCollider2D>();
		clickCollider.offset = transform.position + new Vector3(width / 2.0f, height / 2.0f) - new Vector3(0.5f, 0.5f);
		clickCollider.size = new Vector2(width, height);

		int pathTileType = Random.Range(0, floorTiles.Length - 1);
		int borderTileType;
		do {
			borderTileType = Random.Range(0, floorTiles.Length - 1);
		} while (borderTileType == pathTileType);
		int otherTileType;
		do {
			otherTileType = Random.Range(0, floorTiles.Length - 1);
		} while (otherTileType == pathTileType || otherTileType == borderTileType);

		//Create tiles
		generatedTiles = new GameObject[width * height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int ind = getTileIndex(x, y);
				GameObject tile = GameObject.Instantiate(floorTileObject);
				tile.name = "Tile " + x + ", " + y;
				generatedTiles[ind] = tile;
				tile.transform.position = transform.position + new Vector3(x, y, 1);
			}
		}
		//Move the base into the correct location
		playerBase.transform.position = transform.position + new Vector3(9, 9);
		enemySpawner.transform.position = transform.position + new Vector3(1, 1);
	}

	void Update() {
	}

	int getTileIndex(int x, int y) {
		return y * height + x;
	}
}