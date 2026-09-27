using Godot;
using System;

public partial class ShopIcon : Node2D {
	public string towerName;

	[ExportCategory("Components")]
	[Export] public Sprite2D icon;
	[Export] public ButtonTemplate costButton;
	[Export] public Label towerNameLabel;
	[Export] public Label buyCostLabel;
	[Export] public Label useCostLabel;

	public override void _Process(double delta) {
		if (PlayerManager.instance.GetMoney() < TowerStats.TowerDictionary[towerName].buyPrice) {
			if (costButton.GetAreaState()) {
				costButton.ChangeClickability();
				icon.Modulate = new Color(0.4f, 0.4f, 0.4f, 1);
			}

			costButton.Modulate = new Color(0.4f, 0.4f, 0.4f, 1);
		}

		else {
			if (!costButton.GetAreaState()) {
				costButton.ChangeClickability();
				icon.Modulate = new Color(1, 1, 1, 1);
			}

			costButton.Modulate = new Color(1, 1, 1, 1);
		}
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (costButton.GetClickability()) {
				PurchaseBlueprint();
			}
		}
	}

	public static void CreateShopIcon(string name, int index) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Tower Icons/Shop Icon.tscn");
		ShopIcon blueprint = (ShopIcon)prefab.Instantiate();
		blueprint.Name = name + " Blueprint";
		Shop.instance.GetNode("Tower Shop").AddChild(blueprint);

		// positioning element
		Vector2 screenPos = new Vector2(90, 100);
		screenPos.X += 154.33f * index;
		blueprint.Position = screenPos;

		blueprint.towerName = name;
		blueprint.towerNameLabel.Text = name;
		blueprint.buyCostLabel.Text = TowerStats.TowerDictionary[name].buyPrice.ToString();
		blueprint.useCostLabel.Text = TowerStats.TowerDictionary[name].usePrice.ToString();

		// getting texture location
		Rect2 iconLocation  = new Rect2(0, 0, 32, 32);
		float textureWidth = blueprint.icon.Texture.GetWidth() / 32;
		iconLocation.Position = new Vector2(TowerStats.TowerDictionary[name].index % textureWidth, 0);
		while(textureWidth > (blueprint.icon.Texture.GetWidth() / 32)) {
			iconLocation.Position = iconLocation.Position + new Vector2(0, 1);
			textureWidth -= blueprint.icon.Texture.GetWidth() / 32;
		}

		blueprint.icon.RegionRect = iconLocation;
	}

	private void PurchaseBlueprint() {
		PlayerManager.instance.towers.Add(towerName);
		PlayerManager.instance.UpdateTowers();
		PlayerManager.instance.ChangeMoney(-TowerStats.TowerDictionary[towerName].buyPrice);
		this.QueueFree();
	}
}
