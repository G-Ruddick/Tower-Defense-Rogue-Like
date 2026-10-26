using Godot;
using System;

public partial class GameOver : Control {
	[Export] public ButtonTemplate restartButton;
	[Export] public ButtonTemplate quitButton;

	public override void _Ready() {
		this.Visible = false;
	}

	public override void _Process(double delta) {
		if (!this.Visible  && PlayerManager.instance.GetLives() <= 0) {
			EndGame();
		}
	}

	public override void _Input(InputEvent @event) {
		if (!this.Visible) {
			return;
		}

		if (@event.IsActionPressed("Select")) {
			if (restartButton.GetClickability()) {
				GetTree().Paused = false;
				GetTree().ReloadCurrentScene();
			}

			if (quitButton.GetClickability()) {
				GetTree().Paused = false;
				GetTree().ChangeSceneToFile("res://Scenes/Title Screen.tscn");
			}
		}
	}

	public void EndGame() {
		this.Visible = true;
		GetTree().Paused = true;
		GD.Print("Game Over");
	}
}
