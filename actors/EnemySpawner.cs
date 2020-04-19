using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    IList<PackedScene> EnemyTypes;

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


            if (Util.random() * 3 < delta)
            {
                var numEnemies = GetTree().Root.FindChildrenByType<EnemyShip>().Count;

                if (numEnemies < 0)
                {
                    Console.WriteLine($"Spawning enemy {EnemyTypes}");

                    var toSpawn = EnemyTypes[Util.RandInt(0, EnemyTypes.Count)];

                    Console.WriteLine("Determining offset");
                    var offset = new Vector3(Util.random() - 0.5f, 0, Util.random() - 0.5f).Normalized() * 80;

                    Console.WriteLine("About to instantiate");
                    var enemy = (RigidBody)toSpawn.Instance();
                    GetTree().Root.AddChild(enemy);
                    Console.WriteLine("Spawn is complete, setting location");
                    enemy.SetGlobalLocation(ps.GetGlobalLocation() + offset);
                }
            }
        }
    }
}
