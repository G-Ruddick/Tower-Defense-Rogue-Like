using Godot;
using Godot.Collections;
using System;
using System.Reflection;

public partial class UpgradeNode : Node2D {
	private int currentLevel = 0;
	private bool clickable = true;

	[Export] private int upgradeNumber;
	[Export] private int cost;
	[Export] private int upgradeCostChange;
	[Export] private float abilityValueChanged;
	[Export] private Array<AnimatedSprite2D> levels;
	[Export] private Area2D mouseArea;
	[Export] private Node2D description;
	[Export] private Label costLabel;

	public override void _Ready() {
		description.Visible = false;
		clickable = false;

		ChangeCost(0);
		mouseArea.MouseEntered += DisplayInfo;
		mouseArea.MouseExited += DisplayInfo;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select") && clickable) {
			if (cost <= PlayerManager.instance.GetMoney()) {
				UpgradeAbility(upgradeCostChange);
			}
		}
	}

	private void DisplayInfo() {
		description.Visible = !description.Visible;
		clickable = !clickable;

		if (clickable) {
			this.SelfModulate = new Color(1.25f, 1.25f, 1.25f, 1);
			return;
		}

		this.SelfModulate = Colors.White;
	}

	public void UpgradeAbility(int increaseCost, bool usesMoney = true) {
		if (currentLevel >= levels.Count) {
			return;
		}

		if (usesMoney) {
			PlayerManager.instance.ChangeMoney(-cost);
		}

		ChangeCost(increaseCost);
		ApplyAbility();

		levels[currentLevel++].Play("Owned");
	}

	public void DegradeAbility(int decreaseCost, bool usesMoney = true) {
		if (currentLevel <= 0)  {
			return;
		}

		if (usesMoney) {
			PlayerManager.instance.ChangeMoney((int)(cost * 0.75f));
		}

		ChangeCost(decreaseCost);

		levels[currentLevel--].Play("Unowned");
	}

	private void ApplyAbility() {
		TowerClass tower = GetParent().GetParent().GetParent<TowerClass>();
		string name = tower.towerName.Replace(" ", "");

		// Getting the upgrade function using the parent towers name
		Type type = Type.GetType("TowerUpgrades");
		MethodInfo method = type.GetMethod(name);
		method.Invoke(null, new object[] {tower, upgradeNumber, abilityValueChanged});

		tower.UpdateSellValue(cost - upgradeCostChange);
	}

	public int GetCost() {
		return cost;
	}

	public void ChangeCost(int value) {
		cost += value;
		costLabel.Text = cost.ToString();
	}
}
