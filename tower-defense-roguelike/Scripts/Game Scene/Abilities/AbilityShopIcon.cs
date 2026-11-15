using Godot;
using System;

public partial class AbilityShopIcon : ShopIcon {
	public static void CreateAbilityShopIcon(string name, int index) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Tower Icons/Ability Shop Icon.tscn");
		AbilityShopIcon blueprint = (AbilityShopIcon)prefab.Instantiate();
		blueprint.Name = name + " Blueprint";
		Shop.instance.GetNode("Shop/Ability Shop").AddChild(blueprint);

		// positioning element
		Vector2 screenPos = new Vector2(90, 100);
		screenPos.X += 154.33f * index;
		blueprint.Position = screenPos;

		blueprint.towerName = name;
		blueprint.towerNameLabel.Text = name;
		blueprint.buyCostLabel.Text = AbilityStats.AbilityDictionary[name].buyPrice.ToString();

		blueprint.icon.RegionRect = GetAbilitySprite(blueprint);
	}

	private static Rect2 GetAbilitySprite(ShopIcon blueprint) {
		Rect2 iconLocation  = new Rect2(0, 0, 32, 32);
		float textureWidth = blueprint.icon.Texture.GetWidth() / 32;
		float xValue = AbilityStats.AbilityDictionary[blueprint.towerName].index % textureWidth;
		iconLocation.Position = new Vector2(xValue * 32, 0);

		float y = AbilityStats.AbilityDictionary[blueprint.towerName].index;

		while(y >= textureWidth) {
			iconLocation.Position = iconLocation.Position + new Vector2(0, 32);
			y -= textureWidth;
		}

		return iconLocation;
	}

	protected override void PurchaseBlueprint() {
		PlayerManager.instance.abilities.Add(towerName);
		PlayerManager.instance.UpdateAbilities();
		PlayerManager.instance.ChangeMoney(-AbilityStats.AbilityDictionary[towerName].buyPrice);
		this.QueueFree();
	}
}
