using Godot;

public partial class AbilityClass : Node3D {
    [Export] private string abilityName;
    [Export] private int damage;
    [Export] private float durration;

    [ExportCategory("Components")]
    [Export] private Area3D hitBox;
    [Export] private AnimatedSprite3D abilitySprite;
    [Export] private AnimatedSprite3D cursor;

    public string GetAbilityName() {
        return abilityName;
    }

    public void UseAbility() {

    }
}