using Godot;
using System;

public partial class EndTile : Node3D {
	[Export] public Area3D despawnArea;
	private int enemyLayer = 1 << 2;

	public override void _Ready() {
		despawnArea.AreaEntered += Despawn;
	}

	public void Despawn(Node3D body) {
		GD.Print("object entered");

		if (body is Area3D enemyArea) {
			EnemyClass enemy = enemyArea.GetParent<EnemyClass>();
			enemy.LifeSteal();
		}
	}
}
