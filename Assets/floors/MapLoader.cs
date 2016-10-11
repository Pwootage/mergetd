using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MapLoader {
	const int MAP_COUNT = 6;

	public static GameMap LoadRandomMap() {
		int mapID = UnityEngine.Random.Range(1, MAP_COUNT);

		TextAsset mapInfo = Resources.Load("maps/map" + mapID) as TextAsset;
		TextAsset mapData = Resources.Load("maps/map" + mapID + ".map") as TextAsset;

		if (mapInfo == null || mapData == null) {
			Debug.LogError("Failed to load map id " + mapID);
		}

		string[] mapInfoLines = mapInfo.text.Split('\n');
		int lineNumber = 0;

		int width = 0;
		int height = 0;
		Vector2 pointa = new Vector2();
		Vector2 pointb = new Vector2();
		List<Vector2> path = new List<Vector2>();

		while (lineNumber < mapInfoLines.Length) {
			string l = mapInfoLines[lineNumber];
			lineNumber++;
			if (l == "[size]") {
				string sizeLine = mapInfoLines[lineNumber];
				lineNumber++;

				string[] split = sizeLine.Split(',');
				width = Int32.Parse(split[0]);
				height = Int32.Parse(split[1]);
			} else if (l == "[points]") {
				pointa = StringToVector(mapInfoLines[lineNumber]);
				lineNumber++;
				pointb = StringToVector(mapInfoLines[lineNumber]);
				lineNumber++;
			} else if (l == "[path]") {
				while (lineNumber < mapInfoLines.Length &&
					mapInfoLines[lineNumber].Length > 0 &&
					mapInfoLines[lineNumber][0] != '[') {

					path.Add(StringToVector(mapInfoLines[lineNumber]));
					lineNumber++;
				}
			}
		}

		path.Reverse();

		GameMap.TileType[] tiles = new GameMap.TileType[width * height];
		String[] mapDataLines = mapData.text.Split('\n');
		for (int y = 0; y < width; y++) {
			for (int x = 0; x < width; x++) {
				char tileChar = mapDataLines[y][x];
				GameMap.TileType type = GameMap.TileType.OTHER;
				if (tileChar == '.') {
					type = GameMap.TileType.BORDER;
				} else if (tileChar == '#') {
					type = GameMap.TileType.PATH;
				}

				tiles[GameMap.getTileIndex(x, y, height)] = type;
			}
		}

		//TODO: flip/spin

		return new GameMap(tiles, width, height, pointa, pointb, path);
	}

	private static Vector2 StringToVector(String str) {
		string[] split = str.Split(',');
		int x = Int32.Parse(split[0]);
		int y = Int32.Parse(split[1]);	
		return new Vector2(x, y);
	}
}
