using Godot;

public abstract partial class IBlueprint : Node2D {
	[ExportCategory("Variables")]
	[Export] protected bool enabled;
	[Export] protected bool clickable;
	[Export] protected bool placeable;
	[Export] protected string blueprintName;

	[ExportCategory("Components")]
	[Export] protected Sprite2D icon;
	[Export] protected Area2D button;
	[Export] protected CollisionShape2D clickableArea;
	[Export] public ButtonTemplate sellButton;
	[Export] protected Label costLabel;
	[Export] protected Label nameLabel;

	// Colors
	private Color enabledColor = new Color(1, 1, 1, 1);
	private Color disabledColor = new Color(0.3f, 0.3f, 0.3f, 1);

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
		sellButton.Visible = false;
		button.MouseEntered += () => clickable = true;
		button.MouseExited += () => clickable = false;
	}

	public override void _Process(double delta) {
		if (costLabel != null) {
			costLabel.Text = TowerStats.TowerDictionary[blueprintName].usePrice.ToString();
		}

		CheckIfEnabled();

		if (enabled) { icon.Modulate = enabledColor; }
		else { icon.Modulate = disabledColor; }

		if (enabled) {
			sellButton.Visible = PlayerManager.instance.activeBlueprint == this;
		}
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (sellButton.GetClickability()) {
				SellTower();
				return;
			}

			if (GetClickability()) {
				OnClick();
				return;
			}
		}
	}

	public abstract void CheckIfEnabled();

	public string GetBlueprintName() {
		return blueprintName;
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
			sellButton.Visible = !sellButton.Visible;
			placeable = false;
			PlayerManager.instance.SetBlueprintsInactive();
		}
	}

	// Disabled the button entirely
	public void DisableIcon() {
		enabled = !enabled;
		placeable = false;
		sellButton.Visible = false;
	}

	public abstract void SellTower();

	public void OnClick() {
		if (enabled) {
			if (PlayerManager.instance.activeBlueprint == this) {
				PlayerManager.instance.SetBlueprintsInactive(this);
			}
			else {
				PlayerManager.instance.activeBlueprint = this;
			}

			placeable = !placeable;
			return;
		}

		placeable = false;
		sellButton.Visible = !sellButton.Visible;
	}

	public abstract void PositionBlueprint(int index);
}
