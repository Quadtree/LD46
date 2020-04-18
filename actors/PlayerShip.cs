using Godot;
using System;

public class PlayerShip : RigidBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public float TurnRate = 1.5f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public override void _IntegrateForces(PhysicsDirectBodyState state)
    {
        state.AngularVelocity = new Vector3(0,0,0);

        if (Input.IsActionPressed("Turn Left"))
        {
            state.AngularVelocity = new Vector3(0,TurnRate,0);
        }

        if (Input.IsActionPressed("Turn Right"))
        {
            state.AngularVelocity = new Vector3(0,-TurnRate,0);
        }
    }

    public override void _PhysicsProcess(float delta)
    {



    }
}
