using Godot;
using System.Collections.Generic;

public partial class GameManager: Node {
	public int waveNumber;
	public static int waveCostStart = 50;
	public static int waveCostMultiplier = 3;
	private float resetTimer = 0f;
	public bool waveActive = false;
	
	[Export] private TowerClass currentlySelectedTower;

	public List<GridTile> gridCells = new List<GridTile>();

	[ExportCategory("Scene Components")]
	[Export] private Node EnemyManager;
	[Export] private WinGame winGameManager;
	[Export] private Control shopScreen;
	[Export] private Camera3D camera;
	private Node3D towerIcon;

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
		waveNumber = 0;
	}

	public override void _Process(double delta) {
		UpdateStats();

		if (waveActive) {
			CheckEndOfWaveCondition();
		}

		// handling quick reset
		if (Input.IsActionPressed("Reset")) {
			resetTimer += (float)delta;

			if (resetTimer >= 1f) {
				QuickReset();
			}
		}
		if (Input.IsActionJustReleased("Reset")) {
			resetTimer = 0;
		}

		// checking for tower placement
		if (PlayerManager.instance.activeBlueprint != null) {
			SetGridVisibility(true);

			// creating tower icon
			if (ConditionCheck()) {
				if (towerIcon != null) { 
					towerIcon.QueueFree(); 
				}

				if (PlayerManager.instance.activeBlueprint is AbilityBlueprint) {
					CreateAbilityCursor();
				}
				else if (PlayerManager.instance.activeBlueprint is TowerBlueprint) {
					CreateTowerCursor();
				}
			}

			var query = GetQuery();
			var result = camera.GetWorld3D().DirectSpaceState.IntersectRay(query);
			
			if (result.Count > 0) {
				if (PlayerManager.instance.activeBlueprint is TowerBlueprint) {
					Node3D gameAsset = (result["collider"].AsGodotObject() as Area3D).GetParent<Node3D>();
					towerIcon.Position = gameAsset.Position;
				}
				else if (PlayerManager.instance.activeBlueprint is AbilityBlueprint) {
					towerIcon.Position = (Vector3)result["position"];
				}
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
				return;
			}

			if (nextWaveButton.GetClickability()) {
				NextWaveButton();
				return;
			}

			// map interactions
			if (GetViewport().GuiGetHoveredControl() == null) {
				MapIntereaction();
			}
		}
	}

	private bool ConditionCheck() {
		bool conditionA = towerIcon == null;
		bool conditionB = towerIcon is AbilityClass && (towerIcon as AbilityClass).GetAbilityName() != PlayerManager.instance.activeBlueprint.GetBlueprintName();
		bool conditionC = towerIcon is TowerClass && (towerIcon as TowerClass).towerName != PlayerManager.instance.activeBlueprint.GetBlueprintName();

		return conditionA || conditionB || conditionC;
	}

	private void MapIntereaction() {
		// getting mouse screen position;
		var query = GetQuery();
		
		var result = camera.GetWorld3D().DirectSpaceState.IntersectRay(query);
		if (result.Count > 0) {
			Node3D gameAsset = (result["collider"].AsGodotObject() as Area3D).GetParent<Node3D>();

			IBlueprint blueprint = PlayerManager.instance.activeBlueprint;

			if (blueprint is AbilityBlueprint) {
				AbilityClass.UseAbility(towerIcon.Position, ((AbilityBlueprint)blueprint).GetBlueprintName());
			}
			else {
				PlaceTower(gameAsset, blueprint as TowerBlueprint);
			}
		}
	}

	private void PlaceTower(Node3D gameAsset, TowerBlueprint tower) {
		if (gameAsset.Name.ToString().Contains("Grass")) {
			// placing tower
			if (tower != null) {
				if (!TowerClass.BuyTower(gameAsset.Position, tower.GetBlueprintName())) {
					tower.DisableIcon();
				}
			}
		}
		else if (gameAsset.Name.ToString().Contains("Tower")) {
			// disabling current tower ui
			if (currentlySelectedTower != null && IsInstanceValid(currentlySelectedTower)) {
				currentlySelectedTower.UIToggle();
			}

			// enbling selected tower ui
			if (currentlySelectedTower != gameAsset as TowerClass) {
				currentlySelectedTower = gameAsset as TowerClass;
				currentlySelectedTower.UIToggle();
				return;
			}

			// if the tower clicked is the same tower, set null
			currentlySelectedTower = null;
		}
	}

	private void QuickReset() {
		GD.Print("Reloading Game Scene");
		GetTree().ReloadCurrentScene();
	}

	private void CreateTowerCursor(){
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Towers/" + PlayerManager.instance.activeBlueprint.GetBlueprintName() + ".tscn");
		towerIcon = (TowerClass)prefab.Instantiate();
		towerIcon.Name = "cursor icon";
		PlayerManager.instance.AddChild(towerIcon);
		
		(towerIcon as TowerClass).SetTowerAsCursorObject();
	}

	private void CreateAbilityCursor() {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Abilities/Lightning Strike.tscn");
		towerIcon = (Node3D)prefab.Instantiate();
		towerIcon.Name = "cursor icon";
		PlayerManager.instance.AddChild(towerIcon);
	}

	private void NextWaveButton() {
		waveActive = true;
		waveNumber++;
		StartTile.instance.GetEnemyList();
	}

	private void OpenShop() {
		nextWaveButton.Visible = false;
		Shop.instance.shopOpen = true;
		Shop.instance.Visible = true;
		Shop.instance.rerollCost = 10;
		Shop.instance.Reroll(false);
	}

	private void UpdateStats() {
		playerHealthLabel.Text = "Life: " + PlayerManager.instance.GetLives();
		playerMoneyLabel.Text = "Gold: " + PlayerManager.instance.GetMoney();
		waveNumberLabel.Text = "Wave\n" + waveNumber;
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

	private void CheckEndOfWaveCondition() {
		if (EnemyClass.numberOfEnemies != 0) {
			if (nextWaveButton.GetClickability()) {
				nextWaveButton.ChangeClickability();
			}

			return;
		}

		if (!nextWaveButton.GetClickability()) {
			nextWaveButton.ChangeClickability();
		}

		waveActive = false;
		winGameManager.CheckWinCondition(waveNumber);
		OpenShop();
	}
}
