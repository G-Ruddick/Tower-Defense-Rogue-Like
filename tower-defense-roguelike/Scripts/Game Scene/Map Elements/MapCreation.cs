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
	private Vector2I startTile = new Vector2I();
	private Vector2I endTile = new Vector2I();

	public override void _Ready() {
		// temporary values
		mapHeight = 16;
		mapLength = 20;
		levelType = "Grasslands Tiles";
		
		CreateMap();
		// PrintMap();
		InstantiateMap();
	}

	// Creating Map array
	public void CreateMap() {
		bool validMap = false;
		Vector2I currentTile = new Vector2I(0, 0);
		Vector2I nextTile = new Vector2I(0, 0);

		while (!validMap) {
			mapGrid = new TileTypes[mapHeight, mapLength];

			int pathLength = 0;

			SetStartTile();
			currentTile = SetCurrentTile(startTile);

			bool endReached = false;
			TileTypes direction = TileTypes.Void;

			while (!endReached) {
				List<string> directionOptions = ["North", "East", "South", "West"];
				
				bool validDirection = false;

				if (ValidateEndReached(ref currentTile, ref direction)) {
					endReached = true;
					break;
				}
				
				direction = TileTypes.Void;

				while (!validDirection) {
					if (!directionOptions.Any()) { 
						break; 
					}

					validDirection = ValidateDirection(ref directionOptions, ref direction, ref nextTile, ref currentTile);
				}

				if (!validDirection) {
					endReached = false;
					break;
				}

				mapGrid[currentTile.X, currentTile.Y] = direction;
				currentTile = nextTile;
				pathLength++;
			}
			
			validMap = endReached & ValidateMap(pathLength);
		}
	}

	private void SetStartTile() {
		if (((int)GD.Randi() % 2) == 0) {
			startTile.X = (int)(GD.Randi() % (mapHeight - 1));
			startTile.Y = (int)(GD.Randi() % 2) == 0 ? 0 : (mapLength - 1);
		}
		else {
			startTile.X = (int)(GD.Randi() % 2) == 0 ? 0 : (mapHeight - 1);
			startTile.Y = (int)(GD.Randi() % (mapLength - 1));
		}
	}

	private Vector2I SetCurrentTile(Vector2I currentTile) {
		if (currentTile.X == 0) {
			mapGrid[currentTile.X, currentTile.Y] = TileTypes.South;
			currentTile.X++;
		}
		else if (currentTile.X == mapHeight - 1) {
			mapGrid[currentTile.X, currentTile.Y] = TileTypes.North;
			currentTile.X--;
			
		}
		
		if (currentTile.Y == 0) {
			mapGrid[currentTile.X, currentTile.Y] = TileTypes.East;
			currentTile.Y++;
		}
		else {
			mapGrid[currentTile.X, currentTile.Y] = TileTypes.West;
			currentTile.Y--;
		}

		return currentTile;
	}

	private bool ValidateEndReached(ref Vector2I currentTile, ref TileTypes direction) {
		if (currentTile.X != 0 && currentTile.X != (mapHeight - 1) && currentTile.Y != 0 && currentTile.Y != (mapLength - 1)) {
			return false;
		}

		endTile.X = currentTile.X;
		endTile.Y = currentTile.Y;

		mapGrid[endTile[0], endTile[1]] = direction;

		return true;
	}

	private bool ValidateDirection(ref List<string> directionOptions, ref TileTypes direction, ref Vector2I nextTile, ref Vector2I currentTile) {
		direction = GetDirection(directionOptions, direction);
		nextTile = MovePosition(currentTile, direction);

		// checking to see if tile is occupied
		if (mapGrid[nextTile.X, nextTile.Y] != 0) {
			directionOptions.Remove(direction.ToString());
			return false;
		}

		return true;
	}

	private TileTypes GetDirection(List<string> directionOptions, TileTypes direction) {
		int randomNumber = (int)(GD.Randi() % directionOptions.Count);
		int repeatChance = (int)GD.Randi() % 4;


		if (directionOptions.Contains(direction.ToString()) && repeatChance != 0) {
			return direction;
		}
		
		direction = Enum.Parse<TileTypes>(directionOptions[randomNumber]);
		return direction;
	}

	private Vector2I MovePosition(Vector2I currentTile, TileTypes direction) {
		Vector2I nextTile = currentTile;

		// updating current tile north or south
		switch (direction) {
			case TileTypes.North:
				nextTile[0]--;
				break;
			case TileTypes.South:
				nextTile[0]++;
				break;
			case TileTypes.East:
				nextTile[1]++;
				break;
			case TileTypes.West:
				nextTile[1]--;
				break;
		}

		return nextTile;
	}

	private bool ValidateMap(int pathLength) {
		bool minLength = pathLength > mapHeight * mapLength / 12;
		bool maxLength = pathLength < mapHeight * mapLength / 3;

		return minLength & maxLength;
	}

	// Map intantiation functions
	public void InstantiateMap() {
		for (int row = 0; row < mapHeight; row++) {
			for (int column = 0; column < mapLength; column++) {
				// Spawn and Despawn Tiles
				if (startTile.X == row && startTile.Y == column) {
					SpawnTile("StartTile", row, column);
				}
				if (endTile.X == row && endTile.Y == column) {
					SpawnTile("EndTile", row, column);
				}

				if (mapGrid[row, column] != TileTypes.Void) {
					SpawnPath("Grasslands Tiles/PathTileStrait", row, column);
				}

				// Grass and grid tiles
				SpawnTile(levelType, "GrassTile", row, column);
				SpawnGrid(row, column);
			}
		}
	}

	private void SpawnTile(string tileLocation, int row, int column) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Tiles/" + tileLocation + ".tscn");
		Node3D tile = (Node3D)prefab.Instantiate();
		tile.Name = tileLocation;
		GetChild(2).AddChild(tile);
		tile.Position = new Vector3(column, 0, row) * 0.32f;
	}
	private void SpawnTile(string path, string name, int row, int column) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Tiles/" + path + "/" + name + ".tscn");
		Node3D tile = (Node3D)prefab.Instantiate();
		tile.Name = name + " " + row + " " + column;
		GetChild(2).AddChild(tile);
		tile.Position = new Vector3(column, 0, row) * 0.32f;
	}

	private void SpawnPath(string pathLocation, int row, int column) {
		// creating new path object
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Tiles/" + pathLocation + ".tscn");
		Node3D pathTile = (Node3D)prefab.Instantiate();
		pathTile.Name = "Path" + mapGrid[row, column].ToString() + " " + row + " " + column;
		GetChild(1).AddChild(pathTile);
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

	private void SpawnGrid(int row, int column) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Tiles/GridTile.tscn");
		GridTile cell = (GridTile)prefab.Instantiate();
		cell.Name = "Grid Cell " + column + " " + row;
		GetChild(0).AddChild(cell);

		cell.Position = new Vector3(column, 0, row) * 0.32f;
		GameManager.gameManager.gridCells.Add(cell);
	}

	public void PrintMap() {
		for (int y = 0; y < mapHeight; y++)
		{
			string row = "";

			for (int x = 0; x < mapLength; x++)
			{
				if (mapGrid[y, x] == 0) {
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

		GD.Print("\n\n");
	}
}
