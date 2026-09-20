using Godot;
using System;

public partial class GenerativeTowerClass : Node {
	[ExportCategory("Variables")]
	[Export] public float generateTime;

	private void Generate() {
		GD.Print("Attacking");
	}
}
