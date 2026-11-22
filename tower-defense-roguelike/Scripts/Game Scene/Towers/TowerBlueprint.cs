using Godot;

public partial class TowerBlueprint : IBlueprint {
	public override void CheckIfEnabled() {
		if (PlayerManager.instance.GetMoney() < TowerStats.TowerDictionary[blueprintName].usePrice) { 
			enabled = false; 
			if (PlayerManager.instance.activeBlueprint == this) {
				PlayerManager.instance.SetBlueprintsInactive(this);
			}

			return;
		}

		enabled = true; 
	}

	public override void PositionBlueprint(int index) {
		float x = 102f, x1 = 380f, x2 = 788f;
		Vector2 position = new Vector2(0, 122);

		if (PlayerManager.instance.towers.Count % 2 == 1) {
			position.X = (x2 + x1) / 2;

			if (index < (float)PlayerManager.instance.towers.Count / 2) {
				position.X -= x * index;
			}
			else if (index > (float)PlayerManager.instance.towers.Count / 2) {
				position.X += x * (PlayerManager.instance.towers.Count - index);
			}

			this.Position = position;
			return;
		}
	
		if (index <= PlayerManager.instance.towers.Count / 2) {
			position.X = (x2 + x1) / 2 - (x * (PlayerManager.instance.towers.Count / 2 - index)) + (x / 2);
			this.Position = position;
			return;
		}
		
		position.X = x1 + (x * index) + (x / 2);

		this.Position = position;
		return;
	}

	public override void SellTower() {
		PlayerManager.instance.ChangeMoney((int)Mathf.Ceil(TowerStats.TowerDictionary[blueprintName].buyPrice * 0.75f));
		PlayerManager.instance.towers.Remove(blueprintName);
		PlayerManager.instance.UpdateTowers();

		PlayerManager.instance.SetBlueprintsInactive();
		QueueFree();
	}

	public static TowerBlueprint CreateBlueprint(string name) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Tower Icons/Tower Blueprint.tscn");
		TowerBlueprint blueprint = prefab.Instantiate() as TowerBlueprint;
		blueprint.Name = name + " Blueprint";
		PlayerManager.instance.playerTowersNode.AddChild(blueprint);

		blueprint.blueprintName = name;
		blueprint.nameLabel.Text = name.Replace(" Tower", "");
		blueprint.costLabel.Text = TowerStats.TowerDictionary[name].usePrice.ToString();

		blueprint.icon.RegionRect = GetSprite(blueprint);

		return blueprint;
	}

	private static Rect2 GetSprite(TowerBlueprint blueprint) {
		Rect2 iconLocation  = new Rect2(0, 0, 32, 32);
		float textureWidth = blueprint.icon.Texture.GetWidth() / 32;
		float xValue = TowerStats.TowerDictionary[blueprint.blueprintName].index % textureWidth;
		iconLocation.Position = new Vector2(xValue * 32, 0);

		float y = TowerStats.TowerDictionary[blueprint.blueprintName].index;

		while(y >= textureWidth) {
			iconLocation.Position = iconLocation.Position + new Vector2(0, 32);
			y -= textureWidth;
		}

		return iconLocation;
	}
}
