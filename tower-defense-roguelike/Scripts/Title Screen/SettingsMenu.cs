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
	public static SettingsMenu instance {
		get; 
		private set;
	}

	public override void _Ready() {
		instance = this;

		instance.Visible = false;
		mainMenu.Visible = true;
		soundMenu.Visible = false;
		gameplayMenu.Visible = false;

		OnChangeScene();
		this.VisibilityChanged += OnVisibilityChanged;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (returnButton.GetClickability()) {
				ReturnButton();
			}

			if (quitButton.GetClickability()) {
				QuitGameButton();
			}

			if (leaveGameButton.GetClickability()) {
				LeaveGameButton();
			}

			if (soundSettingsButton.GetClickability()) {
				SoundSettingsButton();
			}

			if (gamePlaySettingsButton.GetClickability()) {
				GameplaySettingsButton();
			}
		}
	}

	public void OnChangeScene(string sceneName = "TitleScreen") {
		if (sceneName == "TitleScreen") {
			leaveGameButton.Visible = false;
			quitButton.Visible = true;
		}
		else {
			leaveGameButton.Visible = true;
			quitButton.Visible = false;
		}
	}

	private void ReturnButton() {
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

	private void QuitGameButton() {
		GD.Print("Quiting Game");
		GetTree().Quit();
	}

	private void LeaveGameButton() {
		GD.Print("Returning to Menu");
		this.Visible = false;

		OnChangeScene();
		GetTree().ChangeSceneToFile("res://Scenes/Title Screen.tscn");
	}

	private void SoundSettingsButton() {
		GD.Print("Opening Sound Settings");
		mainMenu.Visible = false;
		soundMenu.Visible = true;
	}

	private void GameplaySettingsButton() {
		GD.Print("Opening Sound Settings");
		mainMenu.Visible = false;
		gameplayMenu.Visible = true;
	}

	private void OnVisibilityChanged() {
		// GD.Print(!GetTree().Paused);
		GetTree().Paused = this.Visible;
	}
}
