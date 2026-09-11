using Godot;
using System;

public partial class SettingsMenu : CanvasLayer {
	[ExportCategory("Menus")]
	[Export] public Node2D mainMenu;
	[Export] public Node2D soundMenu;
	[Export] public Node2D gameplayMenu;

	[ExportCategory("Main Menu")]
	[Export] public ButtonTemplate quitButton;
	[Export] public ButtonTemplate leaveGameButton;
	[Export] public ButtonTemplate returnButton;
	[Export] public ButtonTemplate soundSettingsButton;
	[Export] public ButtonTemplate gamePlaySettingsButton;

	[ExportCategory("Sound Menu")]
	[Export] private Slider masterVolumeSlider;
	[Export] private Slider musicVolumeSlider;
	[Export] private Slider sfxVolumeSlider;
	[Export] private Slider voicesVolumeSlider;

	[ExportCategory("Gameplay Menu")]
	[Export] private Slider brightnessSlider;
	[Export] private Slider gameSpeedSlider;

	// singleton object
	public static SettingsMenu settingsMenu {
		get; 
		private set;
	}

	public override void _Ready() {
		settingsMenu = this;

		settingsMenu.Visible = false;
		mainMenu.Visible = true;
		soundMenu.Visible = false;
		gameplayMenu.Visible = false;

		if (GetTree().CurrentScene.Name == "TitleScreen") {
			leaveGameButton.Visible = false;
			quitButton.Visible = true;
		}
		else {
			leaveGameButton.Visible = true;
			quitButton.Visible = false;
		}
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (returnButton.GetClickability()) {
				GD.Print("Return");

				if (soundMenu.Visible || gameplayMenu.Visible) {
					soundMenu.Visible = false;
					gameplayMenu.Visible = false;
					mainMenu.Visible = true;
				}
				else {
					this.Visible = false;
				}
			}

			if (quitButton.GetClickability()) {
				GD.Print("Quiting Game");
				GetTree().Quit();
			}
			if (leaveGameButton.GetClickability()) {
				GD.Print("Returning to Menu");
				GetTree().ChangeSceneToFile("res://Scenes/Title Screen.tscn");
			}

			if (soundSettingsButton.GetClickability()) {
				GD.Print("Opening Sound Settings");
				mainMenu.Visible = false;
				soundMenu.Visible = true;
			}

			if (gamePlaySettingsButton.GetClickability()) {
				GD.Print("Opening Sound Settings");
				mainMenu.Visible = false;
				gameplayMenu.Visible = true;
			}
		}
	}
}
