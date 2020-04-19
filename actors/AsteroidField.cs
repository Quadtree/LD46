using Godot;
using System;

public class AsteroidField : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    int BigAsteroids;

    [Export]
    int MediumAsteroids;

    [Export]
    int SmallAsteroids;

    [Export]
    float FieldRadius;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CallDeferred(nameof(SpawnAll));
    }

    public void SpawnAll()
    {
        DoSpawn(BigAsteroids, (PackedScene)GD.Load("res://actors/AsteroidLarge.tscn"));
        DoSpawn(MediumAsteroids, (PackedScene)GD.Load("res://actors/Asteroid.tscn"));
        DoSpawn(SmallAsteroids, (PackedScene)GD.Load("res://actors/AsteroidSmall.tscn"));
    }

    public void DoSpawn(int num, PackedScene typ)
    {
        for (int i=0;i<num;++i)
        {
            float angle = (float)(Util.random() * Math.PI * 2);
            float dist = Util.random() * FieldRadius;

            var nu = (Spatial)typ.Instance();
            GetTree().Root.AddChild(nu);
            nu.SetGlobalLocation(this.GetGlobalLocation() + new Vector3((float)Math.Sin(angle) * dist, 0, (float)Math.Cos(angle) * dist));
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//
//  }
}
