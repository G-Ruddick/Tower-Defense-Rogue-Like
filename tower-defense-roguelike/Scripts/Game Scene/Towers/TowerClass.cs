using Godot;
using System;

public partial class TowerClass : Node {
	[ExportCategory("Variables")]
	[Export] public string towerName;
	[Export] public int towerCost;
	[Export] public bool aquatic; // determines if tower can be placed on water, land, or both

	[ExportCategory("Components")]
	[Export] private Sprite3D towerSprite;
	[Export] public Area3D towerRadius;

	// Destroying Tower
	public void SellTower(bool x) {
		if (x) {
			// dont give refund
		}
		else {
			// give refund
			// playermoney += GetCost() * .75f
		}
		
		this.QueueFree();
	}
}
