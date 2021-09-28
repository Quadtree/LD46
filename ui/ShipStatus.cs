using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public class ShipStatus : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    float[] recentFrameTimes = new float[300];

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var ps = GetTree().Root.FindChildByType<PlayerShip>();

        var pmh = GetTree().Root.FindChildByType<PlayerMoneyHolder>();

        for (int i=recentFrameTimes.Length - 1;i>=1;--i)
        {
            recentFrameTimes[i] = recentFrameTimes[i - 1];
        }

        recentFrameTimes[0] = delta;

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
            Text += $"     {Engine.GetFramesPerSecond()} FPS {(int)(recentFrameTimes.Max() * 1000)}ms      {GetTree().Root.GetChildCount()} Nodes";
        }
    }
}
