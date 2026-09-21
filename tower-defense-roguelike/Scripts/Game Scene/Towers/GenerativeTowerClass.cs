using Godot;
using System;

public partial class GenerativeTowerClass : Node3D {
	[ExportCategory("Variables")]
	[Export] public float generateTime;

	private void Generate() {
		GD.Print("Attacking");
	}
}
