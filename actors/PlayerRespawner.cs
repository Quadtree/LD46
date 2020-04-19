using Godot;
using System;

public class PlayerRespawner : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public float RespawnTime = 5f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        RespawnTime -= delta;

        if (RespawnTime <= 0)
        {
            var ns = ((PackedScene)GD.Load("res://actors/PlayerRespawner.tscn")).Instance();
            GetTree().Root.AddChild(ns);
        }
    }
}
