using Godot;
using System;

public class ShipStatus : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var ps = GetTree().Root.FindChildByType<PlayerShip>();

        var pmh = GetTree().Root.FindChildByType<PlayerMoneyHolder>();

        if (ps != null)
        {
            Text = $"{(int)(ps.HP * 100)}%    ${pmh.Money}";
        }
        else
        {
            Text = "Reconstructing...";
        }

        if (OS.IsDebugBuild())
        {
            Text += $"     {Engine.GetFramesPerSecond()} FPS      {GetTree().Root.GetChildren().Count} Nodes";
        }
    }
}
