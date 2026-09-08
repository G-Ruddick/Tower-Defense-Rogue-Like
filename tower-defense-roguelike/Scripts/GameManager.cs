using Godot;

public partial class GameManager: Node {
    // singleton
    public static GameManager gameManager {
        get; 
        private set;
    }

    [Export] public MapCreation mapCreator;

    public override void _Ready() {
        gameManager = this;

        mapCreator = GetNode<MapCreation>("../MapCreator");

        mapCreator.createMap();
    }
}