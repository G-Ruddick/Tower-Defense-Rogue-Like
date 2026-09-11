using Godot;
using System;

public partial class Shop : Control {
	public bool shopOpen;

	[ExportCategory("Buttons")]
	[Export] private ButtonTemplate leaveButton;
	[Export] private ButtonTemplate mapButton;
	[Export] private ButtonTemplate rerollButton;

	// singleton
	public static Shop shop {
		get;
		private set;
	}

	public override void _Ready() {
		shop = this;
		shopOpen = false;
		this.Visible = false;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (leaveButton.GetClickability()) {
				GameManager.gameManager.nextWaveButton.Visible = true;

				this.Visible = false;
				shopOpen = false;
			}

			if (mapButton.GetClickability()) {
				ViewMap();
			}

			if (rerollButton.GetClickability()) {
				Reroll();
			}
		}
	}

	public void Reroll() {
		GD.Print("Rerolling Shop");
	}

	public void ViewMap() {
		GD.Print("Displaying Map");
	} 
}
