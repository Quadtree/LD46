using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    List<PackedScene> EnemyTypes;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var ps = GetTree().Root.FindChildByType<PlayerShip>();

        if (ps != null)
        {
            if (Util.random() * 10 < delta)
            {
                Console.WriteLine($"Spawning enemy {EnemyTypes}");

                var toSpawn = EnemyTypes[Util.RandInt(0, EnemyTypes.Count)];

                Console.WriteLine("Determining offset");
                var offset = new Vector3(Util.random() - 0.5f, 0, Util.random() - 0.5f).Normalized() * 60;

                Console.WriteLine("About to instantiate");
                var enemy = (RigidBody)toSpawn.Instance();
                Console.WriteLine("Spawn is complete, setting location");
                enemy.SetGlobalLocation(ps.GetGlobalLocation() + offset);
            }
        }
    }
}
