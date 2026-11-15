using Godot;

public partial class AbilityBlueprint : IBlueprint {
	[Export] private float cooldownTime = 10.0f;
	private bool onCoolDown = false;

	public override void CheckIfEnabled() {
		if (onCoolDown) {
			enabled = false;
			return;
		}

		enabled = true;
	}

	public void UseAbility() {
		onCoolDown = true;
		PlayerManager.instance.activeBlueprint = null;
		OnPlace();
		AbilityCooldown();
	}

	public override void PositionBlueprint(int index) {
		float x = 110f, x1 = 119f, x2 = 64f;
		Vector2 position = new Vector2(0, 575);

		if (PlayerManager.instance.abilities.Count < 2) {
			position.X = x1;
			this.Position = position;
			
			return;
		}

		position.X = x * index + x2;
		this.Position = position;

		return;
	}

	public override void SellTower() {
		PlayerManager.instance.ChangeMoney((int)Mathf.Ceil(AbilityStats.AbilityDictionary[blueprintName].buyPrice * 0.75f));
		PlayerManager.instance.abilities.Remove(blueprintName);
		PlayerManager.instance.UpdateAbilities();

		PlayerManager.instance.SetBlueprintsInactive();
		QueueFree();
	}

	private async void AbilityCooldown() {
		if (!onCoolDown) {
			return;
		}

		await ToSignal(GetTree().CreateTimer(cooldownTime, processAlways: false), "timeout");
		onCoolDown = false;
	}

	public static AbilityBlueprint CreateBlueprint(string name) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Tower Icons/Ability Blueprint.tscn");
		AbilityBlueprint blueprint = prefab.Instantiate() as AbilityBlueprint;
		blueprint.Name = name + " Blueprint";
		PlayerManager.instance.playerAbilitiesNode.AddChild(blueprint);
		blueprint.nameLabel.Text = name;
		blueprint.blueprintName = name;
		blueprint.icon.RegionRect = GetSprite(blueprint);
		blueprint.cooldownTime = AbilityStats.AbilityDictionary[name].cooldown;

		return blueprint;
	}

	private static Rect2 GetSprite(AbilityBlueprint blueprint) {
		Rect2 iconLocation  = new Rect2(0, 0, 32, 32);
		float textureWidth = blueprint.icon.Texture.GetWidth() / 32;
		float xValue = AbilityStats.AbilityDictionary[blueprint.blueprintName].index % textureWidth;
		iconLocation.Position = new Vector2(xValue * 32, 0);

		float y = AbilityStats.AbilityDictionary[blueprint.blueprintName].index;

		while(y >= textureWidth) {
			iconLocation.Position = iconLocation.Position + new Vector2(0, 32);
			y -= textureWidth;
		}

		return iconLocation;
	}
}
