using Godot;
using System;

public partial class UIValueChangeIndicator : Label {
	public static void CreateLabel(int value, Vector2 transform) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/UI Value Change Indicator.tscn");
		var newLabel = (UIValueChangeIndicator)prefab.Instantiate();
		GameManager.gameManager.GetParent().GetNode("UI").AddChild(newLabel);

		newLabel.Position = transform;
		newLabel.SetValue(value);
		newLabel.Move();
	}

	private void SetValue(int value) {
		if (value == 0) {
			QueueFree();
			return;
		}

		if (value > 0) {
			this.Modulate = new Color(0, 1, 0, 0.5f);
			this.Text = "+" + value.ToString();
			return;
		}

		this.Modulate = new Color(1, 0, 0, 0.5f);
		this.Text = "-" + value.ToString();
	}

	private async void Move() {
		int timeMoving = 25;
		while (timeMoving-- > 0) {
			this.Position += Vector2.Down * 2;
			this.Modulate += new Color(0, 0, 0, -0.02f);

			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}

		QueueFree();
	}
}
