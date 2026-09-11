using Godot;
using System;

public partial class SceneManager : Node {
	[ExportCategory("Buttons")]
	[Export] private ButtonTemplate settingsButton;
	[Export] private ButtonTemplate playButton;
	[Export] private ButtonTemplate quitButton;

	public override void _Ready() {
		// settingsButton
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (settingsButton.GetClickability()) {
				SettingsMenu.settingsMenu.Visible = true;

				playButton.ChangeClickability();
				quitButton.ChangeClickability();
				settingsButton.ChangeClickability();
			}
			
			if (SettingsMenu.settingsMenu.returnButton.GetClickability() && SettingsMenu.settingsMenu.mainMenu.Visible) {
				playButton.ChangeClickability();
				quitButton.ChangeClickability();
				settingsButton.ChangeClickability();
			}

			if (quitButton.GetClickability()) {
				GD.Print("Quiting Game");
				GetTree().Quit();
			}

			if (playButton.GetClickability()) {
				GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
			}
		}
	}
}
