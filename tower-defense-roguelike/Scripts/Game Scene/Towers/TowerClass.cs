using Godot;
using System;

public partial class TowerClass : Node3D {
	[ExportCategory("Variables")]
	[Export] public string towerName;
	[Export] public int towerCost;
	[Export] public bool aquatic; // determines if tower can be placed on water, land, or both

	public static int towersplaced = 0;

	[ExportCategory("Components")]
	[Export] private Sprite3D towerSprite;
	[Export] public Area3D towerRadius;
	[Export] public Control UIElement;

	[ExportCategory("UI")]
	[Export] private ButtonTemplate sellButton;

	public override void _Ready() {
		UIElement.Visible = false;
		
		// getting tower stats
		towerCost = TowerStats.TowerDictionary[towerName].usePrice;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (sellButton.GetClickability()) {
				SellTower();
			}
		}
	}

	// Destroying Tower
	public void SellTower() {
		PlayerManager.instance.ChangeMoney((int)Mathf.Ceil(towerCost * 0.75f));		
		this.QueueFree();
	}

	public static bool BuyTower(Vector3 position, string name) {
		if (TowerStats.TowerDictionary[name].usePrice > PlayerManager.instance.GetMoney()) {
			return false;
		}
		
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Towers/" + name + ".tscn");
		TowerClass newTower = (TowerClass)prefab.Instantiate();
		newTower.Name = name + towersplaced++;
		PlayerManager.instance.AddChild(newTower);

		// tower position
		newTower.Position = position;

		// removing money from player
		PlayerManager.instance.ChangeMoney(-newTower.towerCost);

		if (TowerStats.TowerDictionary[name].usePrice > PlayerManager.instance.GetMoney()) {
			PlayerManager.instance.GetActiveTower().OnClick();
		}

		return true;
	}

	public void UIToggle() {
		UIElement.Visible = ! UIElement.Visible;
	}
}
