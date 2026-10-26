using Godot;
using System.Collections.Generic;
using System;

public partial class PlayerManager : Node {
	[ExportCategory("Player Stats")]
	[Export] private int money;
	[Export] private int lives; 
	[Export] private int maxNumberOfTowers; 

	public List<string> towers = new List<string>();
	public List<Node2D> abilities = new List<Node2D>();
	[Export] public Node playerTowersNode;

	// singleton
	public static PlayerManager instance {
		get;
		private set;
	}

	public override void _Ready() {
		instance = this;

		SetLives(20);
		SetMoney(200);
		maxNumberOfTowers = 5;

		towers.Add("Archer Tower");
		towers.Add("Mortar Tower");
		towers.Add("Mage Tower");
		UpdateTowers();
	}
	
	// Money Functions
	public int GetMoney() {
		return money;
	}
	public void SetMoney(int value) {
		money = value;
	}
	public void ChangeMoney(int value) {
		money += value;
	}

	// Live functions
	public int GetLives() {
		return lives;
	}
	public void SetLives(int value) {
		lives = value;
	}
	public void ChangeLives(int value) {
		lives += value;
	}

	// adding towers to the players inventory
	public void UpdateTowers() {
		// resetting towers
		foreach (TowerBlueprint tower in playerTowersNode.GetChildren()) {
			tower.QueueFree();
		}

		for(int i = 0; i < towers.Count; i++) {
			TowerBlueprint blueprint = TowerBlueprint.CreateBlueprint(towers[i]);

			blueprint.Position = PositionBlueprint(i);
		}
	}

	private Vector2 PositionBlueprint(int index) {
		float x = 102f, x1 = 380f, x2 = 788f;
		Vector2 position = new Vector2(0, 96);

		if (towers.Count % 2 == 1) {
			if (index == (float)towers.Count / 2) {
				position.X = (x2 + x1) / 2;
			}
			else if (index < (float)towers.Count / 2) {
				position.X = ((x2 + x1) / 2) - (x * index);
			}
			else if (index > (float)towers.Count / 2) {
				position.X = (x2 + x1) / 2 + (x * (towers.Count - index));
			}
		}
		else {
			if (index <= towers.Count / 2) {
				position.X = (x2 + x1) / 2 - (x * (towers.Count / 2 - index)) + (x / 2);
			}
			else if (index > towers.Count / 2) {
				position.X = x1 + (x * index) + (x / 2);
			}
		}

		return position;
	}

	public TowerBlueprint GetActiveTower() {
		foreach(TowerBlueprint tower in playerTowersNode.GetChildren()) {
			if (tower.GetPlaceable()) {
				return tower;
			}
		}

		return null;
	}
	
	public void SetTowersInactive(TowerBlueprint blueprint = null) {
		foreach(TowerBlueprint tower in playerTowersNode.GetChildren()) {
			if (tower.GetPlaceable() && tower != blueprint) {
				tower.ChangePlaceable();
			}
		}
	}
}
