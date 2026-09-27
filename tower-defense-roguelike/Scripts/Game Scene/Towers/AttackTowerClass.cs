using Godot;
using System;

public partial class AttackTowerClass : Node3D {
	[ExportCategory("Variables")]
	[Export] public float Damage;
	[Export] public float reloadTime;
	[Export] public bool enabled;

	[ExportCategory("Components")]
	[Export] public Area3D attackArea;
	[Export] public CollisionShape3D attackRange;

	private void Attack() {
		GD.Print("Attacking");
	}
}
