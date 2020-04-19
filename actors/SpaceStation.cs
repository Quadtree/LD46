using Godot;
using System;

public class SpaceStation : KinematicBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    Material MaterialOverride;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CallDeferred(nameof(SetMaterials));
    }

    public void SetMaterials()
    {
        foreach (var mi in this.FindChildrenByType<MeshInstance>())
        {
            mi.MaterialOverride = MaterialOverride;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        RotateY(0.1f * delta);
    }
}
