using Godot;
using System;

public partial class CameraMovever : Camera3D {
	[Export] private float zoomSpeed = 0.1f;
	[Export] private float moveSpeed = 200.0f;
	private bool movingScreen;

	[Export] private MapCreation mapCreator;
	
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
			MoveCamera(mouseMotion);
		}

		// zoom
		if (@event.IsActionPressed("Scroll Down")) {
			if (this.Size < mapCreator.mapHeight * mapCreator.mapLength / 60) {
				this.Size += zoomSpeed;
			}
		}
		if (@event.IsActionPressed("Scroll Up")) {
			if (this.Size > 0.5f) {
				this.Size -= zoomSpeed;
			}
		}
	}

	private void MoveCamera(InputEventMouseMotion mouseMotion) {
		Vector3 position = new Vector3(-mouseMotion.Relative.X, 0, -mouseMotion.Relative.Y) / moveSpeed + this.GlobalPosition;

		if (position.X <= 0 || position.X >= (mapCreator.mapLength - 1) * 0.32f) {
			position.X = this.GlobalPosition.X;
		}

		if (position.Z <= 6.5f || position.Z >= mapCreator.mapHeight * 0.32f + 5.5) {
			position.Z = this.GlobalPosition.Z;
		}

		this.GlobalPosition = position;
	}
}
