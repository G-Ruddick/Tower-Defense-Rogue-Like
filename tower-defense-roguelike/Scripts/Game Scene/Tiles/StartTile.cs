using Godot;
using System;

public partial class StartTile : Node3D {
	public static StartTile instance;
	
	[Export] private Area2D tileArea;

	public override void _Ready() {
		instance = this;
		CallDeferred(nameof(SpawnEnemies));
	}

	public override void _Process(double delta) {

	}

	public void SpawnEnemies() {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Enemies/Goblin.tscn");
		Node3D enemy = (Node3D)prefab.Instantiate();
		GetNode("../../Enemies").AddChild(enemy);
		enemy.Position = this.Position;
	}
}
