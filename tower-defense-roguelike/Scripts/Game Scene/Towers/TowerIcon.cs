using Godot;

public partial class TowerIcon : Node2D {
	[ExportCategory("Variables")]
	[Export] private bool enabled;
	[Export] private bool clickable;
	[Export] private bool placeable;
	[Export] private string towerName;

	[ExportCategory("Components")]
	[Export] private Sprite2D icon;
	[Export] private Area2D button;
	[Export] private CollisionShape2D clickableArea;
	[Export] public ButtonTemplate SellButton;
	[Export] private Label Cost;

	// Colors
	private Color enabledColor = new Color(1, 1, 1, 1);
	private Color disabledColor = new Color(0.2f, 0.2f, 0.2f, 1);
	private Color placeableColor = new Color(1.35f, 1.35f, 1.35f, 1);

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
		placeable = false;
		SellButton.Visible = false;
		Cost.Text = TowerStats.TowerDictionary[towerName].usePrice.ToString();

		button.MouseEntered += () => clickable = true;
		button.MouseExited += () => clickable = false;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (SellButton.GetClickability()) {
				SellTower();
				return;
			}

			if (GetClickability()) {
				OnClick();
				return;
			}
		}
	}

	public override void _Process(double delta) {
		if (PlayerManager.instance.GetMoney() < TowerStats.TowerDictionary[towerName].usePrice) { enabled = false; }
		else { enabled = true; }

		if (enabled) { icon.Modulate = placeable ? placeableColor : enabledColor; }
		else { icon.Modulate = disabledColor; }
	}

	public override void _ExitTree() {
		PlayerManager.instance.CallDeferred(nameof(PlayerManager.instance.UpdateTowers));
	}

	public string GetTowerName() {
		return towerName;
	}

	// Clickability for if the button can be touched
	public bool GetClickability() {
		return clickable;
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

	public void OnPlace() {
		if (placeable) {
			SellButton.Visible = !SellButton.Visible;
			placeable = false;
		}
	}

	// Disabled the button entirely
	public void DisableIcon() {
		enabled = !enabled;
		SellButton.Visible = false;
	}

	public void SellTower() {
		PlayerManager.instance.ChangeMoney((int)Mathf.Ceil(TowerStats.TowerDictionary[towerName].buyPrice * 0.75f));
		PlayerManager.instance.towers.Remove(GetTowerName());
		this.QueueFree();
	}

	public void OnClick() {
		SellButton.Visible = !SellButton.Visible;
		if (enabled) {
			placeable = !placeable;
		}
		else {
			placeable = false;
		}
	}
}
