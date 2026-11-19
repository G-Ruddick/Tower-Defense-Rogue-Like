using Godot;
using System;

public partial class CameraMovever : Camera3D {
	[Export] private float zoomSpeed = 0.1f;
	[Export] private float moveSpeed = 200.0f;
	private bool movingScreen;

	public override void _Ready() {
		movingScreen = false;
	}

	public override void _Input(InputEvent @event) {
		// moving the screen
		if (@event.IsActionPressed("Move")) {
			movingScreen = true;
		}
		if (@event.IsActionReleased("Move")) {
			movingScreen = false;
		}
		
		if (@event is InputEventMouseMotion mouseMotion && movingScreen) {
			Vector3 position = new Vector3(-mouseMotion.Relative.X, 0, -mouseMotion.Relative.Y) / moveSpeed + this.GlobalPosition;
			
			if (CheckIfInsideGameMap(position)) { 
				this.GlobalPosition = position;
			}
		}

		// zoom
		if (@event.IsActionPressed("Scroll Down")) {
			this.Size += zoomSpeed;
		}
		if (@event.IsActionPressed("Scroll Up")) {
			this.Size -= zoomSpeed;
		}
	}

	private bool CheckIfInsideGameMap(Vector3 position) {
		if (position.X <= 0 || position.Z <= 6.5) {
			return false;
		}

		return true;
	}
}
