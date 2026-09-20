using Godot;

public partial class TowerIcon : Node2D {
	[ExportCategory("Variables")]
	[Export] public bool enabled;
	[Export] public bool clickable;
	[Export] public bool placeable;
	[Export] private string towerName;
	
	[ExportCategory("Components")]
	[Export] private Sprite2D icon;
	[Export] private Area2D button;
	[Export] private CollisionShape2D clickableArea;
	[Export] public ButtonTemplate SellButton;
	[Export] private Label Cost;

	public override void _Ready() {
		if (icon == null) {
			GD.PrintErr(this.Name + "'s Sprite2D has no assigned sprite.");
			GetTree().Quit();
		}
		if (clickableArea == null) {
			GD.PrintErr(this.Name + "'s CollisionShape2D has no assigned shape.");
			GetTree().Quit();
		}

		enabled = true;
		clickable = false;
		placeable = true;
		SellButton.Visible = false;

		button.MouseEntered += () => clickable = true;
		button.MouseExited += () => clickable = false;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (SellButton.GetClickability()) {
				SellTower();
			}

			if (GetClickability()) {
				OnClick();
			}
		}
	}

	public override void _ExitTree() {
		PlayerManager.instance.CallDeferred(nameof(PlayerManager.instance.UpdateTowers));
	}

	public string GetTowerName() {
		return towerName;
	}

	// Clickability for if the button can be touched
	public bool GetClickability() {
		return (enabled & clickable);
	}
	public void ChangeClickability() {
		clickable = !clickable;
	}
	
	// determines if the player is to be placing a tower or not
	public bool GetPlaceable() {
		return placeable;
	}
	public void ChangePlaceable() {
		placeable = !placeable;
	}

	// Disabled the button entirely
	public void DisableIcon() {
		icon.Modulate = enabled ? new Color(20, 20, 20, 1) : new Color(1, 1, 1, 1);
		button.Visible = !button.Visible;
		enabled = !enabled;
	}

	public void SellTower() {
		PlayerManager.instance.ChangeMoney(25);
		PlayerManager.instance.towers.Remove(GetTowerName());
		this.QueueFree();
	}

	public void OnClick() {
		SellButton.Visible = !SellButton.Visible;
		icon.Modulate = SellButton.Visible ? new Color(1.35f, 1.35f, 1.35f, 1) : new Color(1, 1, 1, 1);

		// if (placeable) {
			
		// }
		// else {

		// }
	}
}
