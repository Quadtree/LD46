using Godot;
using System;

public class TitleScreen : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    float NoInputTime = 1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _Input(InputEvent @event)
    {
        if (NoInputTime <= 0 && (@event is InputEventKey || @event is InputEventMouseButton))
        {
            GetTree().ClearAndChangeScene("res://maps/default.tscn");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        NoInputTime -= delta;
    }
}
