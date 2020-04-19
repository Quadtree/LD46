using Godot;
using System;

public class MissionStatus : Label
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
            if (ps.NextCargo != null && ps.NextStation != null)
            {
                Text = $"Collect the {ps.NextCargo.Name} from {ps.NextStation.Name}";
            }
            else if (ps.Cargo != null && ps.NextStation != null)
            {
                Text = $"Take the {ps.Cargo.Name} to {ps.NextStation.Name}";
            }
            else
            {
                Text = "";
            }
        }
    }
}
