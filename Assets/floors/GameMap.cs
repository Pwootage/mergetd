﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMap {
	public enum TileType {
		PATH,
		BORDER,
		OTHER
	}

	public TileType[] tiles;
	public GameObject[] towers;
	public int width;
	public int height;
	public Vector2 pointa;
	public Vector2 pointb;
	public List<Vector2> path = new List<Vector2>();

	public GameMap(TileType[] tiles, int width, int height, Vector2 pointa, Vector2 pointb, List<Vector2> path) {
		this.tiles = tiles;
		towers = new GameObject[tiles.Length];
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

	public static int getTileIndex(int x, int y, int height) {
		return y * height + x;
	}
}
