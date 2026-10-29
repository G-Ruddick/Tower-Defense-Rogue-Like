using Godot;
using System;

public partial class SceneManager : Node {
	[ExportCategory("Buttons")]
	[Export] private ButtonTemplate settingsButton;
	[Export] private ButtonTemplate playButton;
	[Export] private ButtonTemplate quitButton;


	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (settingsButton.GetClickability()) {
				SettingsButton();
			}

			if (quitButton.GetClickability()) {
				QuitButton();
			}

			if (playButton.GetClickability()) {
				PlayButton();
			}
		}
	}

	private void SettingsButton() {
		SettingsMenu.instance.Visible = true;
	}

	private void QuitButton() {
		GD.Print("Quiting Game");
		GetTree().Quit();
	}

	private void PlayButton() {
		SettingsMenu.instance.OnChangeScene("Game");
		GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
	}
}
