using Godot;
using System;

public class MissionPointer : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        var ps = GetTree().Root.FindChildByType<PlayerShip>();

        if (ps != null && ps.NextStation != null)
        {
            this.Visible = true;

            var dist = ps.GetGlobalLocation().DistanceTo(ps.NextStation.GetGlobalLocation());

            if (dist > 25)
            {
                var posDelta = (ps.NextStation.GetGlobalLocation() - ps.GetGlobalLocation()).Normalized();
                this.SetGlobalLocation(ps.GetGlobalLocation() + posDelta * 20);
                this.LookAt(ps.NextStation.GetGlobalLocation(), new Vector3(0, 1, 0));
            } else {
                this.SetGlobalLocation(ps.NextStation.GetGlobalLocation() + new Vector3(0, 5, 0));
                this.LookAt(ps.NextStation.GetGlobalLocation(), new Vector3(0, 0, 1));
            }
        } else {
            this.Visible = false;
        }
    }
}
