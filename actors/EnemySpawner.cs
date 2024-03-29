using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public class EnemySpawner : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    Array<PackedScene> EnemyTypes;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        var ps = GetTree().Root.FindChildByType<PlayerShip>();

        //Console.WriteLine($"Num nodes {GetTree().Root.GetChildren().Count}");

        if (ps != null)
        {
            if (Util.random() * 3 < delta)
            {
                var numEnemies = GetTree().Root.FindChildrenByType<EnemyShip>().Count();

                if (numEnemies < 3)
                {
                    Console.WriteLine($"Spawning enemy {EnemyTypes}");

                    var toSpawn = EnemyTypes[Util.RandInt(0, EnemyTypes.Count)];

                    Console.WriteLine("Determining offset");
                    var offset = new Vector3(Util.random() - 0.5f, 0, Util.random() - 0.5f).Normalized() * 80;

                    Console.WriteLine("About to instantiate");
                    var enemy = (RigidBody)((PackedScene)toSpawn).Instance();
                    GetTree().Root.AddChild(enemy);
                    Console.WriteLine("Spawn is complete, setting location");
                    enemy.SetGlobalLocation(ps.GetGlobalLocation() + offset);
                }
            }
        }
    }
}
