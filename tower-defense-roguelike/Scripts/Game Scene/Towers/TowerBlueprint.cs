using Godot;

public partial class TowerBlueprint : Node2D {
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
	[Export] private Label towerCostLabel;
	[Export] private Label towerNameLabel;

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
		SellButton.Visible = false;
		button.MouseEntered += () => clickable = true;
		button.MouseExited += () => clickable = false;
	}

	public override void _Process(double delta) {
		towerCostLabel.Text = TowerStats.TowerDictionary[towerName].usePrice.ToString();

		if (PlayerManager.instance.GetMoney() < TowerStats.TowerDictionary[towerName].usePrice) { enabled = false; }
		else { enabled = true; }

		if (enabled) { icon.Modulate = enabledColor; }
		else { icon.Modulate = disabledColor; }

		if (enabled) {
			SellButton.Visible = PlayerManager.instance.GetActiveTower() == this;
		}
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			foreach(Node i in PlayerManager.instance.playerTowersNode.GetChildren()) {
				GD.Print(i.Name);
			}

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

	// public override void _ExitTree() {
	// 	PlayerManager.instance.CallDeferred(nameof(PlayerManager.instance.UpdateTowers));
	// }

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
		placeable = false;
		SellButton.Visible = false;
	}

	public void SellTower() {
		PlayerManager.instance.ChangeMoney((int)Mathf.Ceil(TowerStats.TowerDictionary[towerName].buyPrice * 0.75f));
		PlayerManager.instance.towers.Remove(towerName);
		PlayerManager.instance.UpdateTowers();

		// PlayerManager.instance.SetTowersInactive();
		QueueFree();
	}

	public void OnClick() {
		PlayerManager.instance.SetTowersInactive(this);
		

		if (enabled) {
			placeable = !placeable;
			return;
		}

		placeable = false;
		SellButton.Visible = !SellButton.Visible;
	}

	public static TowerBlueprint CreateBlueprint(string name) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Tower Icons/Tower Blueprint.tscn");
		TowerBlueprint blueprint = prefab.Instantiate() as TowerBlueprint;
		blueprint.Name = name + " Blueprint";
		PlayerManager.instance.playerTowersNode.AddChild(blueprint);

		blueprint.towerName = name;
		blueprint.towerNameLabel.Text = name;
		blueprint.towerCostLabel.Text = TowerStats.TowerDictionary[name].usePrice.ToString();

		blueprint.icon.RegionRect = GetSprite(blueprint);

		return blueprint;
	}

	private static Rect2 GetSprite(TowerBlueprint blueprint) {
		Rect2 iconLocation  = new Rect2(0, 0, 32, 32);
		float textureWidth = blueprint.icon.Texture.GetWidth() / 32;
		float xValue = TowerStats.TowerDictionary[blueprint.towerName].index % textureWidth;
		iconLocation.Position = new Vector2(xValue * 32, 0);
		
		while(textureWidth > (blueprint.icon.Texture.GetWidth() / 32)) {
			iconLocation.Position = iconLocation.Position + new Vector2(0, 1);
			textureWidth -= blueprint.icon.Texture.GetWidth() / 32;
		}

		return iconLocation;
	}
}
