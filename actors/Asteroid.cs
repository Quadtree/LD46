using Godot;
using System;

public class Asteroid : RigidBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalTransform = new Transform(
            new Quat(new Vector3(Util.random() * 360, Util.random() * 360, Util.random() * 360)),
            GlobalTransform.origin
        );
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
