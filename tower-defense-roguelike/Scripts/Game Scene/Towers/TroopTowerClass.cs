using Godot;
using System;

public partial class TroopTowerClass : Node3D {
	[ExportCategory("Variables")]
	[Export] public int troopCount;
	[Export] public float respawnTime;

	[ExportCategory("Components")]
	[Export] public Area3D troopRange;

	private void Spawn() {
		GD.Print("Spawning");
	}
}
