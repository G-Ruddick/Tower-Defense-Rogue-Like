using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class MapCreation : Node {
	// types of times
	public enum TileTypes {Void, North, East, South, West};

	// map size
	public int mapHeight;
	public int mapLength;

	// map level type
	public string levelType;

	private TileTypes[,] mapGrid;
	private int[] startTile = new int[2];
	private int[] endTile = new int[2];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		// temporary values
		mapHeight = 16;
		mapLength = 20;
		levelType = "res://Prefabs/Grasslands Tiles/";
		
		CreateMap();
		PrintMap();
		InstantiateMap();
	}

	// Creating Map array
	public void CreateMap() {
		bool validMap = false;
		int[] currentTile = new int[2];
		int[] nextTile = new int[2];

		while (!validMap) {
			mapGrid = new TileTypes[mapHeight, mapLength];

			int pathLength = 0;

			// setting starting tile for enemy spawning
			if (((int)GD.Randi() % 2) == 0) {
				startTile[0] = (int)(GD.Randi() % (mapHeight - 1));
				startTile[1] = (int)(GD.Randi() % 2) == 0 ? 0 : (mapLength - 1);
			}
			else {
				startTile[0] = (int)(GD.Randi() % 2) == 0 ? 0 : (mapHeight - 1);
				startTile[1] = (int)(GD.Randi() % (mapLength - 1));
			}

			currentTile[0] = startTile[0];
			currentTile[1] = startTile[1];

			// setting starting direction
			if (currentTile[0] == 0) {
				mapGrid[currentTile[0], currentTile[1]] = TileTypes.South;
				currentTile[0]++;
			}
			else if (currentTile[0] == mapHeight - 1) {
				mapGrid[currentTile[0], currentTile[1]] = TileTypes.North;
				currentTile[0]--;
				
			}
			else if (currentTile[1] == 0) {
				mapGrid[currentTile[0], currentTile[1]] = TileTypes.East;
				currentTile[1]++;
			}
			else {
				mapGrid[currentTile[0], currentTile[1]] = TileTypes.West;
				currentTile[1]--;
			}

			bool endReached = false;
			TileTypes direction = TileTypes.Void;

			while (!endReached) {
				List<string> directionOptions = ["North", "East", "South", "West"];
				
				bool validDirection = false;

				// checking for edge of the map
				if (currentTile[0] == 0 || currentTile[0] == (mapHeight - 1) || currentTile[1] == 0 || currentTile[1] == (mapLength - 1)) {
					endTile[0] = currentTile[0];
					endTile[1] = currentTile[1];

					mapGrid[endTile[0], endTile[1]] = direction;

					endReached = true;
					break;
				}
				
				direction = TileTypes.Void;

				while (!validDirection) {
					// checking for no more available directions
					if (!directionOptions.Any()) {
						break;
					}

					if (directionOptions.Contains(direction.ToString())) {
						if ((int)GD.Randi() % 4 == 0) {
							direction = Enum.Parse<TileTypes>(directionOptions[(int)(GD.Randi() % directionOptions.Count)]);
						}
					}
					else {
						direction = Enum.Parse<TileTypes>(directionOptions[(int)(GD.Randi() % directionOptions.Count)]);
					}

					
					// resetting next tile
					nextTile[0] = currentTile[0];
					nextTile[1] = currentTile[1];

					// updating current tile north or south
					if (direction == TileTypes.North) {
						nextTile[0]--;
					}
					else if (direction == TileTypes.South) {
						nextTile[0]++;
					}
					// updating current tile east or west
					if (direction == TileTypes.East) {
						nextTile[1]++;
					}
					else if (direction == TileTypes.West) {
						nextTile[1]--;
					}

					// checking to see if tile is occupied
					if (mapGrid[nextTile[0], nextTile[1]] != 0) {
						directionOptions.Remove(direction.ToString());
					}
					else {
						validDirection = true;
					}
				}
				if (!validDirection) {
					endReached = false;
					break;
				}

				mapGrid[currentTile[0], currentTile[1]] = direction;

				currentTile[0] = nextTile[0];
				currentTile[1] = nextTile[1];

				pathLength++;
			}
			
			// checking for valid map
			if (endReached && (pathLength > mapHeight * mapLength / 12) && (pathLength < mapHeight * mapLength / 3)) {
				validMap = true;
			}
		}
	}

	public void InstantiateMap() {
		PackedScene tile = GD.Load<PackedScene>("res://Prefabs/Tiles/Grasslands Tiles/GrassTile.tscn");
		PackedScene path = GD.Load<PackedScene>("res://Prefabs/Tiles/Grasslands Tiles/PathTileStrait.tscn");
		PackedScene spawn = GD.Load<PackedScene>("res://Prefabs/Tiles/StartTile.tscn");
		PackedScene despawn = GD.Load<PackedScene>("res://Prefabs/Tiles/EndTile.tscn");
		
		for (int row = 0; row < mapHeight; row++) {
			for (int column = 0; column < mapLength; column++) {
				// Spawn and Despawn Tiles
				if (startTile[0] == row && startTile[1] == column) {
					Node3D spawnPoint = (Node3D)spawn.Instantiate();
					spawnPoint.Name = "Start Tile";
					AddChild(spawnPoint);
					spawnPoint.Position = new Vector3(column, 0, row) * 0.32f;
				}
				if (endTile[0] == row && endTile[1] == column) {
					Node3D despawnPoint = (Node3D)despawn.Instantiate();
					despawnPoint.Name = "Despawn Tile";
					AddChild(despawnPoint);
					despawnPoint.Position = new Vector3(column, 0, row) * 0.32f;
				}

				// Grass and pth tile
				Node3D grassTile = (Node3D)tile.Instantiate();
				grassTile.Name = "Grass Tile" + row + " " + column;
				AddChild(grassTile);
				grassTile.Position = new Vector3(column, 0, row) * 0.32f;

				if (mapGrid[row, column] != TileTypes.Void) {
					// creating new path object
					Node3D pathTile = (Node3D)path.Instantiate();
					pathTile.Name = "Path" + mapGrid[row, column].ToString() + " " + row + " " + column;
					AddChild(pathTile);
					pathTile.Position = new Vector3(column, 0, row) * 0.32f;

					// setting rotation
					if (mapGrid[row, column] == TileTypes.North) {
						pathTile.RotationDegrees = new Vector3(0f, 180f, 0f);
					}

					else if (mapGrid[row, column] == TileTypes.South) {
						pathTile.RotationDegrees = new Vector3(0f, 0f, 0f);
					}

					else if (mapGrid[row, column] == TileTypes.West) {
						pathTile.RotationDegrees = new Vector3(0f, -90f, 0f);
					}

					else if (mapGrid[row, column] == TileTypes.East) {
						pathTile.RotationDegrees = new Vector3(0f, 90f, 0f);
					}
				}
			}
		}
	}

	public void PrintMap() {
		for (int y = 0; y < mapHeight; y++)
		{
			string row = "";

			for (int x = 0; x < mapLength; x++)
			{
				if ((int)mapGrid[y, x] == 0) {
					row += ". ";
				}

				else {
					if (mapGrid[y, x] == TileTypes.North) {
						row += "^ ";
					}
					else if (mapGrid[y, x] == TileTypes.South) {
						row += "v ";
					}
					else if (mapGrid[y, x] == TileTypes.East) {
						row += "> ";
					}
					else if (mapGrid[y, x] == TileTypes.West) {
						row += "< ";
					}
				}
			}

			GD.Print(row);
		}
		GD.Print("\n");
		GD.Print("\n");
	}
}
