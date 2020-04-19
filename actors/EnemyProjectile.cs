using Godot;
using System;

public class EnemyProjectile : RigidBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private float TTL = 10f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("body_entered", this, nameof(OnBodyEntered));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        TTL -= delta;

        if (TTL <= 0)
        {
            QueueFree();
        }
    }

    public void OnBodyEntered(Node body)
    {
        Console.WriteLine("Hit something!");

        if (body is PlayerShip)
        {
            ((PlayerShip)body).TakeDamage(0.2f);
        }

        QueueFree();
    }
}
