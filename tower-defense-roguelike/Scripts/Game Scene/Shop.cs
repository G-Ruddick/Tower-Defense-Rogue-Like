using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Shop : Control {
	public bool shopOpen;
	public int rerollCost = 10;

	[ExportCategory("Buttons")]
	[Export] private ButtonTemplate leaveButton;
	[Export] private ButtonTemplate mapButton;
	[Export] private ButtonTemplate rerollButton;

	// singleton
	public static Shop instance {
		get;
		private set;
	}

	public override void _Ready() {
		instance = this;
		shopOpen = false;
		this.Visible = false;
	}

	public override void _Process(double delta) {
		rerollButton.textBox.Text = "Reroll: " + rerollCost;

		if (PlayerManager.instance.GetMoney() < rerollCost) {
			if (rerollButton.GetAreaState()) {
				rerollButton.ChangeClickability();
				rerollButton.Modulate = new Color(0.4f, 0.4f, 0.4f, 1);
			}
		}
		else {
			if (!rerollButton.GetAreaState()) {
				rerollButton.ChangeClickability();
				rerollButton.Modulate = new Color(1, 1, 1, 1);
			}
		}
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (leaveButton.GetClickability()) {
				GameManager.gameManager.nextWaveButton.Visible = true;

				this.Visible = false;
				shopOpen = false;

				StartTile.instance.SpawnEnemies();
				StartTile.instance.SpawnEnemies(0.05f, -0.1f);
			}

			if (mapButton.GetClickability()) {
				ViewMap();
			}

			if (rerollButton.GetClickability()) {
				Reroll(true);
			}
		}
	}

	public void Reroll(bool costMoney) {
		// resetting shop
		foreach (ShopIcon blueprint in GetNode("Tower Shop").GetChildren()) {
			blueprint.QueueFree();
		}

		// removing players money;
		if (costMoney) {
			PlayerManager.instance.ChangeMoney(-rerollCost);
			rerollCost += 15;
		}

		List<string> availableTowers = TowerStats.TowerDictionary.Keys.ToList();

		for (int i = 0; i < 4; i++) {
			string name = "";
			while (name == "") {
				int index  = (int)GD.RandRange(0, availableTowers.Count - 1);
				GD.Print(index);
				name = availableTowers[index];

				for (int j = 0; j < PlayerManager.instance.towers.Count; j++) {
					GD.Print(name);
					GD.Print(PlayerManager.instance.towers[j]);
					if (name == PlayerManager.instance.towers[j]) {
						name = "";
						availableTowers.Remove(availableTowers[index]);
						break;
					}
				}

				if (availableTowers.Count == 0) { break; }
			}
			
			if (availableTowers.Count == 0) { break; }

			ShopIcon.CreateShopIcon(name, i);
			availableTowers.Remove(name);

			if (availableTowers.Count == 0) { break; }
		}
		
		GD.Print("Rerolling Shop");
	}

	public void ViewMap() {
		GD.Print("Displaying Map");
	} 
}
