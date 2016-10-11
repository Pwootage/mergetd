using UnityEngine;
using System.Collections;

public class FloorGenerator : MonoBehaviour {
	public int width = 20;
	public int height = 20;
	public Sprite[] floorTiles = { };
	public GameObject floorTileObject;
	public GameObject playerBase;
	public GameObject enemySpawner;
	private GameMap map;
	private GameObject[] generatedTiles;

	void Start() {
		//Setup click collider
		BoxCollider2D clickCollider = gameObject.GetComponent<BoxCollider2D>();
		clickCollider.offset = transform.position + new Vector3(width / 2.0f, height / 2.0f) - new Vector3(0.5f, 0.5f);
		clickCollider.size = new Vector2(width, height);

		//Pick tile types
		int pathTileType = Random.Range(0, floorTiles.Length - 1);
		int borderTileType;
		do {
			borderTileType = Random.Range(0, floorTiles.Length - 1);
		} while (borderTileType == pathTileType);
		int otherTileType;
		do {
			otherTileType = Random.Range(0, floorTiles.Length - 1);
		} while (otherTileType == pathTileType || otherTileType == borderTileType);
			
		GameMap map = MapLoader.LoadRandomMap();

		//Create tiles
		generatedTiles = new GameObject[width * height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int ind = map.getTileIndex(x, y);
				GameMap.TileType type = map.tiles[ind];
				GameObject tile = GameObject.Instantiate(floorTileObject);
				FloorTile floorTile = tile.GetComponent<FloorTile>();

				switch (type) {
					case GameMap.TileType.PATH:
						floorTile.SetSprite(floorTiles[pathTileType]);
						break;
					case GameMap.TileType.BORDER:
						floorTile.SetSprite(floorTiles[borderTileType]);
						break;
					case GameMap.TileType.OTHER:
						floorTile.SetSprite(floorTiles[otherTileType]);
						break;
				}

				tile.name = "Tile " + x + ", " + y;
				generatedTiles[ind] = tile;
				tile.transform.position = transform.position + new Vector3(x, y, 1);
			}
		}

		//Move the base into the correct location
		playerBase.transform.position = transform.position + new Vector3(map.pointa.x, map.pointa.y);
		enemySpawner.transform.position = transform.position + new Vector3(map.pointb.x, map.pointb.y);

		WaveController wave = enemySpawner.GetComponent<WaveController>();
		wave.path = map.path;
	}

	void Update() {
	}
}