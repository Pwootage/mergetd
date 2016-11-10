using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMap {
	public enum TileType {
		PATH,
		BORDER,
		OTHER
	}

	private TileType[] tiles;
	private GameObject[] towers;
	private SpecialTileEffect[] effects;
	public int width;
	public int height;
	public Vector2 pointa;
	public Vector2 pointb;
	public List<Vector2> path = new List<Vector2>();

	public GameMap(TileType[] tiles, SpecialTileEffect[] effects, int width, int height, Vector2 pointa, Vector2 pointb, List<Vector2> path) {
		this.tiles = tiles;
		towers = new GameObject[tiles.Length];
		this.effects = effects;
		this.width = width;
		this.height = height;
		this.pointa = pointa;
		this.pointb = pointb;
		this.path = path;

	}

	public int getTileIndex(int x, int y) {
		return y * height + x;
	}

	public bool isBuildable(int x, int y) {
		return tiles[getTileIndex(x, y)] != TileType.PATH;
	}

	public TileType getTile(int x, int y) {
		return tiles[getTileIndex(x, y)];
	}

	public GameObject getTower(int x, int y) {
		return towers[getTileIndex(x, y)];
	}

	public SpecialTileEffect getSpecialEffect(int x, int y) {
		return effects[getTileIndex(x, y)];
	}

	public void setTower(int x, int y, GameObject tower) {
		towers[getTileIndex(x, y)] = tower;
	}

	public static int getTileIndex(int x, int y, int height) {
		return y * height + x;
	}
}
