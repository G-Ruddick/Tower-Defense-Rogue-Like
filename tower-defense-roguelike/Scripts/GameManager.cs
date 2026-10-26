using Godot;
using System.Collections.Generic;

public partial class GameManager: Node {
	public int waveNumber;
	private bool movingScreen;
	public static int waveCostStart = 50;
	public static int waveCostMultiplier = 3;

	public List<GridTile> gridCells = new List<GridTile>();

	[ExportCategory("Scene Components")]
	[Export] private Control shopScreen;
	[Export] private Camera3D camera;
	[Export] private TowerClass towerIcon;

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
				
		// checking for tower placement
		if (PlayerManager.instance.GetActiveTower() != null) {
			SetGridVisibility(true);

			// creating tower icon
			if (towerIcon == null || towerIcon.towerName != PlayerManager.instance.GetActiveTower().GetTowerName()) {
				if (towerIcon != null) { 
					towerIcon.QueueFree(); 
				}

				PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Towers/" + PlayerManager.instance.GetActiveTower().GetTowerName() + ".tscn");
				towerIcon = (TowerClass)prefab.Instantiate();
				towerIcon.Name = "cursor icon";
				PlayerManager.instance.AddChild(towerIcon);
				
				towerIcon.SetTowerAsCursorObject();
			}

			var query = GetQuery();
			
			var result = camera.GetWorld3D().DirectSpaceState.IntersectRay(query);
			if (result.Count > 0) {
				Node3D gameAsset = (result["collider"].AsGodotObject() as Area3D).GetParent<Node3D>();
				towerIcon.Position = gameAsset.Position;
			}
		}
		else {
			SetGridVisibility(false);

			if (towerIcon != null) {
				towerIcon.QueueFree();
				towerIcon = null;
			}
		}
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Select")) {
			if (settingsButton.GetClickability()) {
				SettingsMenu.instance.Visible = true;
				DisableUI();
				return;
			}

			if (SettingsMenu.instance.returnButton.GetClickability()) {
				DisableUI();
				return;
			}

			if (nextWaveButton.GetClickability()) {
				NextWaveButton();
				return;
			}

			// map interactions
			if (GetViewport().GuiGetHoveredControl() == null) {
				// getting mouse screen position;
				var query = GetQuery();
				
				var result = camera.GetWorld3D().DirectSpaceState.IntersectRay(query);
				if (result.Count > 0) {
					Node3D gameAsset = (result["collider"].AsGodotObject() as Area3D).GetParent<Node3D>();
					// GD.Print(gameAsset.Name);
					// GD.Print(gameAsset.Position);

					if (gameAsset.Name.ToString().Contains("Grass")) {
						// placing tower
						TowerBlueprint tower = PlayerManager.instance.GetActiveTower();
						if (tower != null) {
							if (!TowerClass.BuyTower(gameAsset.Position, tower.GetTowerName())) {
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

	private void NextWaveButton() {
		nextWaveButton.Visible = false;
		Shop.instance.shopOpen = true;
		Shop.instance.Visible = true;
		Shop.instance.rerollCost = 10;
		Shop.instance.Reroll(false);

		waveNumber++;
	}

	private void DisableUI() {
		settingsButton.ChangeClickability();
		nextWaveButton.ChangeClickability();
	}

	private PhysicsRayQueryParameters3D GetQuery() {
		var mousePosition = GetViewport().GetMousePosition();
		var origin = camera.ProjectRayOrigin(mousePosition);
		var end = origin + camera.ProjectRayNormal(mousePosition) * 1000f;
		var query = PhysicsRayQueryParameters3D.Create(origin, end);
		query.CollisionMask = 2 | 4 | 32;
		query.CollideWithAreas = true;

		return query;
	}

	private void SetGridVisibility(bool visibility) {
		foreach (GridTile cell in gridCells) {
			cell.SetVisibility(visibility);
			
			if (!visibility) {
				continue;
			}

			cell.CheckForObjects();
		}
	}
}
