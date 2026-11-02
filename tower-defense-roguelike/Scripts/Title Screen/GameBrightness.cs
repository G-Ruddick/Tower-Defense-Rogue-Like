using Godot;
using System;

public partial class GameBrightness : Node {
	[Export] private CanvasModulate brightnessController2D;
	[Export] private WorldEnvironment brightnessController3D;

	public override void _Ready() {
		SettingsMenu.instance.brightnessSlider.ValueChanged += ChangeBrightness;

		ChangeBrightness(SettingsMenu.instance.brightnessSlider.Value);
	}

	public void ChangeBrightness(double sliderValue) {
		float value = (float) sliderValue;

		if (brightnessController2D != null) {
			brightnessController2D.Color = new Color(value, value, value, 1);
		}

		if (brightnessController3D != null) {
			brightnessController3D.Environment.TonemapExposure = value;
		}
	}
}
