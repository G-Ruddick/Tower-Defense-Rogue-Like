using Godot;
using System;

public partial class WinGame : Control {
	private int winConditionWaveNumber = 3;
	
	[Export] private ButtonTemplate endlessModeButton;
	[Export] private ButtonTemplate returnToMenuButton;
	
	public override void _Ready() {
		this.Visible = false;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (endlessModeButton.GetClickability()) {
				EndlessModeButtonOnClick();
			}

			if (returnToMenuButton.GetClickability()) {
				ReturnToMenuButtonOnClick();
			}
		}
	}

	public void CheckWinCondition(int waveNumber) {
		if (waveNumber == winConditionWaveNumber) {
			this.Visible = true;
			GetTree().Paused = true;

			// set win condition wave number to a value that cant be checked so 
			// win screen doesnt show up more than once
			winConditionWaveNumber = -100;
		}
	}

	public void SetWinCondition(int waveNumber) {
		winConditionWaveNumber = waveNumber;
	}

	private void EndlessModeButtonOnClick() {
		GetTree().Paused = false;
		this.Visible = false;
	}

	private void ReturnToMenuButtonOnClick() {
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/Title Screen.tscn");
	}
}
