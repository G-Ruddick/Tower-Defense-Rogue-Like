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

		// temporary value assignment
		money = 100;
		lives = 20;
		maxNumberOfTowers = 5;

		towers.Add("Archer Tower");
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
		float x = 102f, x1 = 380f, x2 = 788f;
		Vector2 position = new Vector2(0, 96);

		// resetting towers
		foreach (TowerIcon tower in playerTowersNode.GetChildren()) {
			tower.QueueFree();
		}

		for(int i = 0; i < towers.Count; i++) {
			// creating icon
			PackedScene icon = GD.Load<PackedScene>("res://Prefabs/Tower Icons/" + towers[i] + " Icon.tscn");
			TowerIcon newIcon = (TowerIcon)icon.Instantiate();
			newIcon.Name = towers[i];
			playerTowersNode.AddChild(newIcon);

			// positioning icon;
			if (towers.Count % 2 == 1) {
				if (i == (float)towers.Count / 2) {
					position.X = (x2 + x1) / 2;
				}
				else if (i < (float)towers.Count / 2) {
					position.X = ((x2 + x1) / 2) - (x * i);
				}
				else if (i > (float)towers.Count / 2) {
					position.X = (x2 + x1) / 2 + (x * (towers.Count - i));
				}
			}
			else {
				if (i <= towers.Count / 2) {
					position.X = (x2 + x1) / 2 - (x * (towers.Count / 2 - i)) + (x / 2);
				}
				else if (i > towers.Count / 2) {
					position.X = x1 + (x * i) + (x / 2);
				}
			}

			newIcon.Position = position;
		}
	}
}
