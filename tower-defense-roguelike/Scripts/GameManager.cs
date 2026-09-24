using Godot;

public partial class GameManager: Node {
	private int waveNumber;
	private bool movingScreen;

	[ExportCategory("Scene Components")]
	[Export] private Control shopScreen;
	[Export] private Camera3D camera;

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
		waveNumber = 1;
		movingScreen = false;
	}

	public override void _Process(double delta) {
		// updating labels to the game state
		playerHealthLabel.Text = "Life: " + PlayerManager.instance.GetLives();
		playerMoneyLabel.Text = "Gold: " + PlayerManager.instance.GetMoney();
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

			// map interactions
			if (GetViewport().GuiGetHoveredControl() == null) {
				// getting mouse screen position;
				var mousePosition = GetViewport().GetMousePosition();
				var origin = camera.ProjectRayOrigin(mousePosition);
				var end = origin + camera.ProjectRayNormal(mousePosition) * 1000f;
				var query = PhysicsRayQueryParameters3D.Create(origin, end);
				query.CollisionMask = (1 << 1) | (1 << 2);
				query.CollideWithAreas = true;
				
				var result = camera.GetWorld3D().DirectSpaceState.IntersectRay(query);
				if (result.Count > 0) {
					Node3D gameAsset = (result["collider"].AsGodotObject() as Area3D).GetParent<Node3D>();
					GD.Print(gameAsset.Name);
					GD.Print(gameAsset.Position);

					if (gameAsset.Name.ToString().Contains("Grass")) {
						// placing tower
						TowerIcon tower = PlayerManager.instance.GetActiveTower();
						if (tower != null) {
							if (!TowerClass.BuyTower(gameAsset.Position, tower.GetTowerName())) {
								tower.OnClick();
								tower.DisableIcon();
							}
						}
					}

					else if (gameAsset.Name.ToString().Contains("Tower")) {
						((TowerClass)gameAsset).UIToggle();
					}
				}
			}
		}

		// moving the screen
		if (@event.IsActionPressed("Move")) {
			movingScreen = true;
		}
		if (@event.IsActionReleased("Move")) {
			movingScreen = false;
		}
		if (@event is InputEventMouseMotion mouseMotion && movingScreen) {
			Vector3 position = new Vector3(-mouseMotion.Relative.X, 0, -mouseMotion.Relative.Y) / 200 + camera.GlobalPosition;
			camera.GlobalPosition = position;
			// GD.Print("Moving screen");
		}

		// zoom
		if (@event.IsActionPressed("Scroll Down")) {
			camera.Size += 0.1f;
		}
		if (@event.IsActionPressed("Scroll Up")) {
			camera.Size -= 0.1f;
		}
	}

	private void DisableUI() {
		settingsButton.ChangeClickability();
		nextWaveButton.ChangeClickability();
	}
}