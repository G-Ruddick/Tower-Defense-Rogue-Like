using Godot;
using System;
using System.Collections.Generic;

public partial class StartTile : Node3D {
	public static StartTile instance;
	
	[Export] private Area2D tileArea;

	public override void _Ready() {
		instance = this;
	}

	public async void GetEnemyList() {
		List<string> enemyList = EnemyWaveSpawner.instance.ExportList();

		foreach (string enemy in enemyList) {
			SpawnEnemies(enemy);

			await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		}
	}

	public void SpawnEnemies(string name) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Enemies/" + name + ".tscn");
		EnemyClass enemy = (EnemyClass)prefab.Instantiate();
		GetNode("../../../Enemies").AddChild(enemy);

		enemy.Position = this.Position + new Vector3((float)GD.RandRange(-0.11f, 0.11f), 0.01f, (float)GD.RandRange(-0.11f, 0.11f));
		enemy.walkLocation = enemy.Position;
	}
  
  public void SpawnEnemies(string name, float x, float z) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Enemies/" + name + ".tscn");
		EnemyClass enemy = (EnemyClass)prefab.Instantiate();
		GetNode("../../Enemies").AddChild(enemy);

		enemy.Position = this.Position + new Vector3(x, 0.01f, z);
		enemy.walkLocation = enemy.Position;
	}
}
