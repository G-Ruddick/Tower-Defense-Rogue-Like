using Godot;
using System;

public partial class TowerClass : Node3D {
	[ExportCategory("Variables")]
	[Export] public string towerName;
	[Export] public int towerCost;
	[Export] public bool aquatic; // determines if tower can be placed on water, land, or both

	public static int towersplaced = 0;

	[ExportCategory("Tower Type")]
	[Export] public AttackTowerClass attackClass;
	[Export] public GenerativeTowerClass generateClass;
	[Export] public TroopTowerClass troopClass;

	[ExportCategory("Components")]
	[Export] private Sprite3D towerSprite;
	[Export] public Area3D towerRadius;
	[Export] public Control UIElement;

	[ExportCategory("UI")]
	[Export] private ButtonTemplate sellButton;
	[Export] private Sprite3D rangeSprite;

	public override void _Ready() {
		UIElement.Visible = false;
		rangeSprite.Visible = false;
		
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
			PlayerManager.instance.GetActiveTower().DisableIcon();
		}
		else {			
			PlayerManager.instance.GetActiveTower().OnPlace();
		}


		return true;
	}

	public void UIToggle() {
		UIElement.Visible = ! UIElement.Visible;
		
		if (attackClass != null) {
			attackClass.rangeSprite.Visible = !attackClass.rangeSprite.Visible;
		}
	}

	public void DisableTowerElements() {
		if (attackClass != null) { attackClass.enabled = false; }
		if (generateClass != null) { generateClass.enabled = false; }
		if (troopClass != null) { troopClass.enabled = false; }

	}
	
	public void EnableTowerElements() {
		if (attackClass != null) { attackClass.enabled = true; }
		if (generateClass != null) { generateClass.enabled = true; }
		if (troopClass != null) { troopClass.enabled = true; }

	}

	public void SetTowerAsCursorObject() {
		DisableTowerElements();
		towerRadius.QueueFree();

		if (attackClass != null) {
			rangeSprite.Visible = true;
			rangeSprite.Modulate -= new Color(0.05f, 0.05f, 0.05f, 0.02f);
		}
		
		towerSprite.Modulate -= new Color(0.2f, 0.2f, 0.2f, 0.02f);
	}
}
