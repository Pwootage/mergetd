using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct SpecialFloorTileSprites {
	public  Sprite attackUp;
	public  Sprite rangeUp;
	public  Sprite speedUp;
}



public class FloorController : MonoBehaviour {
	public SpecialFloorTileSprites specialSprites;
	public Sprite[] floorTiles = { };
	public GameObject floorTileObject;
	public GameObject playerBase;
	public GameObject enemySpawner;
	private GameObject[] generatedTiles;
	private GameObject[] generatedSpecialTiles;
    private GameState state;

	void Start() {
        state = GameState.FindInScene();
        state.map = MapLoader.LoadRandomMap();

        GameObject.Find("Main Camera").GetComponent<FloorCamera>().UpdateCamera();

        //Setup click collider
        BoxCollider2D clickCollider = gameObject.GetComponent<BoxCollider2D>();
		clickCollider.offset = transform.position + new Vector3(state.map.width / 2.0f, state.map.height / 2.0f) - new Vector3(0.5f, 0.5f);
		clickCollider.size = new Vector2(state.map.width, state.map.height);

		//Pick tile types
		UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
		int pathTileType = UnityEngine.Random.Range(0, floorTiles.Length);
		int borderTileType;
		do {
			borderTileType = UnityEngine.Random.Range(0, floorTiles.Length);
		} while (borderTileType == pathTileType);
		int otherTileType;
		do {
			otherTileType = UnityEngine.Random.Range(0, floorTiles.Length);
		} while (otherTileType == pathTileType || otherTileType == borderTileType);

		//Create tiles
		generatedTiles = new GameObject[state.map.width * state.map.height];
		generatedSpecialTiles = new GameObject[state.map.width * state.map.height];
		for (int x = 0; x < state.map.width; x++) {
			for (int y = 0; y < state.map.height; y++) {
				int ind = state.map.getTileIndex(x, y);
				GameMap.TileType type = state.map.getTile(x, y);
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



				if (state.map.getSpecialEffect(x, y) != SpecialTileEffect.NONE) {
					GameObject specialTile = GameObject.Instantiate(floorTileObject);
					FloorTile specialFloorTile = specialTile.GetComponent<FloorTile>();
					specialTile.name = "Special Tile " + x + ", " + y;
					generatedSpecialTiles[ind] = specialTile;
					specialTile.transform.position = transform.position + new Vector3(x, y, -3);

					switch (state.map.getSpecialEffect(x, y)) {
						case SpecialTileEffect.ATTACK_UP:
							specialFloorTile.SetSprite(specialSprites.attackUp);
							break;
						case SpecialTileEffect.RANGE_UP:
							specialFloorTile.SetSprite(specialSprites.rangeUp);
							break;
						case SpecialTileEffect.SPEED_UP:
							specialFloorTile.SetSprite(specialSprites.speedUp);
							break;
						case SpecialTileEffect.NONE:
						default:
							break;
					}
				}
			}
		}

		//Move the base into the correct location
		playerBase.transform.position = transform.position + new Vector3(state.map.pointa.x, state.map.pointa.y);
		enemySpawner.transform.position = transform.position + new Vector3(state.map.pointb.x, state.map.pointb.y);

		WaveController wave = enemySpawner.GetComponent<WaveController>();
		wave.path = state.map.path;
	}

	void Update() {
        // Clean up projectiles
	    float buffer = 0.1f;
        Rect gameboard = new Rect(transform.position.x - buffer, transform.position.y - buffer, state.map.width + buffer * 2, state.map.height + buffer * 2);

	    GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");

	    foreach (GameObject projectile in projectiles) {
	        if (!gameboard.Contains(projectile.transform.position)) {
	            Destroy(projectile);
	        }
	    }

	}
}