using Godot;

public partial class GameManager: Node {
	private int waveNumber;

	[ExportCategory("Scene Components")]
	[Export] private Control shopScreen;
	[Export] private PlayerManager playerManager;

	[ExportCategory("Labels")]
	[Export] private Label playerHealthLabel;
	[Export] private Label playerMoneyLabel; 
	[Export] private Label waveNumberLabel;

	[ExportCategory("UI Buttons")]
	[Export] public ButtonTemplate settingsButton;
	[Export] public ButtonTemplate nextWaveButton;

	// singleton
	public static GameManager gameManager {
		get; 
		private set;
	}

	public override void _Ready() {
		gameManager = this;	

		// TEMPORARY VALUES
		playerManager.SetLives(20);
		playerManager.SetMoney(200);
		waveNumber = 1;
	}

	public override void _Process(double delta) {
		// updating labels to the game state
		playerHealthLabel.Text = "Life: " + playerManager.GetLives();
		playerMoneyLabel.Text = "Gold: " + playerManager.GetMoney();
		waveNumberLabel.Text = "Wave\n" + waveNumber;
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (settingsButton.GetClickability()) {
				SettingsMenu.settingsMenu.Visible = true;
				DisableUI();
			}
			if (SettingsMenu.settingsMenu.returnButton.GetClickability()) {
				DisableUI();
			}

			if (nextWaveButton.GetClickability()) {
				nextWaveButton.Visible = false;
				Shop.shop.shopOpen = true;
				Shop.shop.Visible = true;

				waveNumber++;
			}
		}
	}

	private void DisableUI() {
		settingsButton.ChangeClickability();
		nextWaveButton.ChangeClickability();
	}
}