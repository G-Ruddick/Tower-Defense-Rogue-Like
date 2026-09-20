using Godot;
using System;

public partial class PlayerManager : Node {
	[ExportCategory("Player Stats")]
	[Export] private int money;
	[Export] private int lives; 
	[Export] private int numberOfTowers; 

	public Node3D[] towers;

	public override void _Ready() {
		// temporary value assignment
		money = 100;
		lives = 20;
		numberOfTowers = 1;

		towers = new Node3D[numberOfTowers];
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
}
