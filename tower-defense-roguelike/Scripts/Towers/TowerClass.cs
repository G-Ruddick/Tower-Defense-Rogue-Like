using Godot;
using System;

public partial class TowerClass : Node {
	// name of tower 
	protected string towerName;
	public string GetTowerName() {
		return towerName;
	}
	
	// cost to buy and sell tower
	protected int towerCost;
	public int GetCost() {
		return towerCost;
	}
	
	// determines if tower can be placed on water, land, or both
	protected bool? aquatic;
	public bool? GetAquatic() {
		return aquatic;
	}

	// determines if tower is floating or not 
	protected bool floating;
	public bool GetFloating() {
		return floating;
	}

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

	[Export] protected AnimatedSprite2D towerSprite;
	
	protected float towerRange;
	[Export] protected Area2D towerRangeRadius;

	protected int[] towerSize;
	[Export] protected Area2D towerRadius;
}
