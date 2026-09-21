using Godot;

public partial class AssetUiScreenFollow : Control {
	public override void _Process(double delta) {
		Camera3D camera = GetViewport().GetCamera3D();
		Vector2 screenPosition = camera.UnprojectPosition(((Node3D)GetParent()).Position);

		this.Position = screenPosition;
	}
}
