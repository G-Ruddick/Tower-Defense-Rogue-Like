using Godot;
using System;

public partial class StartTile : Node3D {
	public static StartTile instance;
	
	[Export] private Area2D tileArea;

	public override void _Ready() {
		instance = this;
		CallDeferred(nameof(SpawnEnemies));
	}

	public void SpawnEnemies(float x, float z) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Enemies/Goblin.tscn");
		EnemyClass enemy = (EnemyClass)prefab.Instantiate();
		GetNode("../../Enemies").AddChild(enemy);

		enemy.Position = this.Position + new Vector3(x, 0.01f, z);
		enemy.walkLocation = enemy.Position;
	}
	
	public void SpawnEnemies() {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Enemies/Goblin.tscn");
		EnemyClass enemy = (EnemyClass)prefab.Instantiate();
		GetNode("../../Enemies").AddChild(enemy);

		enemy.Position = this.Position + new Vector3((float)GD.RandRange(-0.11f, 0.11f), 0.01f, (float)GD.RandRange(-0.11f, 0.11f));
		enemy.walkLocation = enemy.Position;
	}
}
