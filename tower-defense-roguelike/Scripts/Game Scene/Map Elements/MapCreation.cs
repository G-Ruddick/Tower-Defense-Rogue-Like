using Godot;
using System;
using System.Collections.Generic;

public partial class MapCreation : Node {
	// singleton
	public static MapCreation instance {
		get; 
		private set;
	}

	// types of times
	public enum TileTypes {Void, North, East, South, West};

	// map size
	public int mapHeight;
	public int mapLength;

	private TileTypes[,] mapGrid;
	private int[] startTile = new int[2];
	private int[] endTile = new int[2];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		instance = this;

		mapHeight = 16;
		mapLength = 20;
	}

	// Creating Map array
	public void createMap() {
		bool validMap = false;
		int[] currentTile = new int[2];
		int[] nextTile = new int[2];

		while (!validMap) {
			mapGrid = new TileTypes[mapHeight, mapLength];

			int pathLength = 0;

			// setting starting tile for enemy spawning
			if (((int)GD.Randi() % 2) == 0) {
				startTile[0] = 1 + (int)(GD.Randi() % (mapHeight - 1));
				startTile[1] = ((int)(GD.Randi() % 2) == 0) ? 0 : (mapLength - 1);
			}
			else {
				startTile[0] = ((int)(GD.Randi() % 2) == 0) ? 0 : (mapHeight - 1);
				startTile[1] = 1 + (int)(GD.Randi() % (mapLength - 1));
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
					if (directionOptions.Count == 0) {
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

					if (mapGrid[nextTile[0], nextTile[1]] != 0) {
						directionOptions.Remove(direction.ToString());
					}

					else {
						validDirection = true;
					}
				}

				mapGrid[currentTile[0], currentTile[1]] = direction;

				currentTile[0] = nextTile[0];
				currentTile[1] = nextTile[1];

				pathLength++;
			}
			
			// checking for valid map
			if ((pathLength > mapHeight * mapLength / 12) && (pathLength < mapHeight * mapLength / 3)) {
				validMap = true;
			}
		}
	}

	public void printMap() {
		for (int y = 0; y < mapHeight; y++)
		{
			string row = "";

			for (int x = 0; x < mapLength; x++)
			{
				if ((int)mapGrid[y, x] == 0) {
					row += ". ";
				}

				else {
					row += (int)mapGrid[y, x] + " ";
				}
			}

			GD.Print(row);
		}
		GD.Print("\n");
		GD.Print("\n");
	}
}
