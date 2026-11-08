using Godot;
using System;

public partial class GenerativeTowerClass : Node3D {
	public enum GenerateType { Money };

	[ExportCategory("Variables")]
	[Export] private GenerateType type;
	[Export] private float generateTime;
	[Export] private float generateAmmount;
	[Export] public bool enabled;

	[ExportCategory("Components")]
	[Export] private AnimatedSprite3D animatedComponent;

	public override void _Ready() {
		enabled = GameManager.gameManager.waveActive;
		Generate();
	}

	public override void _Process(double delta) {
		enabled = GameManager.gameManager.waveActive;
	}

	private async void Generate() {
		while (true) {
			if (!enabled) {
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				continue;
			}

			animatedComponent.Play("default");
			GetGeneration();
			
			await ToSignal(animatedComponent, "animation_finished");
			await ToSignal(GetTree().CreateTimer(generateTime, processAlways: false), "timeout");
		}
	}

	private void GetGeneration() {
		switch (type) {
			case GenerateType.Money:
				GD.Print("Generating Gold"); 
				PlayerManager.instance.ChangeMoney((int)generateAmmount);
				break;
		}
	}

	public void ChangeGenerationAmmount(float value) {
		generateAmmount += value;
	}
}
