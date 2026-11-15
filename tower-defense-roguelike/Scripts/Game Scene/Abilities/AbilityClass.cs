using Godot;

public partial class AbilityClass : Node3D {
    [Export] private string abilityName;
    [Export] private int damage = 0;
    [Export] private float durration = 0.1f;

    [ExportCategory("Components")]
    [Export] private Area3D hitbox;
    [Export] private AnimatedSprite3D abilitySprite;
    [Export] private AnimatedSprite3D afterEffectSprite;
    [Export] private int afterEffectPlayOnFrame = 0;
    [Export] private AnimatedSprite3D cursor;

    public override void _Ready() {
        hitbox.Monitoring = false;
        abilitySprite.Visible = false;
        cursor.Visible = true;

        if (afterEffectSprite != null) {
            afterEffectSprite.Visible = false;
            afterEffectSprite.AnimationFinished += CheckIfAbilityFinished;
        }

        hitbox.AreaEntered += DamageEffect;
        abilitySprite.FrameChanged += OnAbilityFrameChanged;
        abilitySprite.AnimationFinished += CheckIfAbilityFinished;
    }

    public string GetAbilityName() {
        return abilityName;
    }

    public void InitializeAbility() {
        cursor.Visible = false;
        abilitySprite.Visible = true;
        abilitySprite.Frame = 0;
        abilitySprite.Play();
    }

    private void DamageEffect(Node3D body) {
		if (body is Area3D) {
			EnemyClass enemy = body.GetParent<Node3D>() as EnemyClass;

			if (enemy != null) {
				enemy.TakeDamage(damage);
			}
		}
	}

    private void OnAbilityFrameChanged() {
        if (abilitySprite.Frame == afterEffectPlayOnFrame) {
            ActivateHitbox();
            
            if (afterEffectSprite != null) {
                afterEffectSprite.Visible = true;
                afterEffectSprite.Play();
            }
        }

    }

    private async void ActivateHitbox() {
        hitbox.Monitoring = true;

        await ToSignal(GetTree().CreateTimer(durration, processAlways: false), "timeout");

        hitbox.Monitoring = false;
    }

    private void CheckIfAbilityFinished() {
        if (abilitySprite.IsPlaying()) {
            return;
        }

        if (afterEffectSprite != null && afterEffectSprite.IsPlaying()) {
            return;
        }

        this.QueueFree();
    }

    public static void UseAbility(Vector3 position, string name) {
		PackedScene prefab = GD.Load<PackedScene>("res://Prefabs/Abilities/" + name + ".tscn");
		AbilityClass ability = (AbilityClass)prefab.Instantiate();
		ability.Name = name;
		PlayerManager.instance.AddChild(ability);

		// tower position
		ability.Position = position;
        ability.InitializeAbility();

        ((AbilityBlueprint)PlayerManager.instance.activeBlueprint).UseAbility();
        PlayerManager.instance.activeBlueprint = null;
	}
}