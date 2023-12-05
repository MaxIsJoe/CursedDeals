using Godot;

namespace MIJGWJ54.Scripts.Player;

public partial class Player : Node
{
    public static Player Instance { get; private set; }

    public int Money = 0;
    public int Karma = 5;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
}