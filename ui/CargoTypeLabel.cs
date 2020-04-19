using Godot;
using System;

public class CargoTypeLabel : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

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
            if (ps.Cargo != null)
                Text = $"Current Cargo: {ps.Cargo.Name} ({(int)(ps.Cargo.HP * 100)}%)";
            else
                Text = "Current Cargo: None";

            if (ps.Cargo != null || ps.NextCargo != null)
            {
                var cg = ps.Cargo != null ? ps.Cargo : ps.NextCargo;

                Text += "\n\n" + cg.Description;
            }
        }
    }
}
