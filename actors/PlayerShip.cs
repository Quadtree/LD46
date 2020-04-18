using Godot;
using System;

public class PlayerShip : RigidBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public float TurnRate = 1.5f;

    [Export]
    public float ThrustPower = 8f;

    int CurrentTurn;

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


        if (CurrentTurn == -1)
        {
            state.AngularVelocity = new Vector3(0,TurnRate,0);
        }
        else if (CurrentTurn == 1)
        {
            state.AngularVelocity = new Vector3(0,-TurnRate,0);
        }
        else
        {
            state.AngularVelocity = new Vector3(0,0,0);
        }

        if (Input.IsActionPressed("Thrust Forward"))
        {
            state.AddCentralForce(-state.Transform.basis.z * ThrustPower);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        var cam = GetViewport().GetCamera();

        var raySrc = cam.ProjectRayOrigin(GetViewport().GetMousePosition());
        var rayNorm = cam.ProjectRayNormal(GetViewport().GetMousePosition());

        var curPos = GetWorld().DirectSpaceState.IntersectRay(raySrc, raySrc + rayNorm * 10000);

        Console.WriteLine($"raySrc={raySrc}, curPos={rayNorm}");

        if (curPos != null && curPos.Contains("position"))
        {
            Console.WriteLine("pos=" + curPos["position"]);

            var pos = (Vector3)curPos["position"];

            var tpv2 = new Vector2(pos.x, pos.z);
            var spv2 = new Vector2(this.GetGlobalLocation().x, this.GetGlobalLocation().y);

            var wantedAngle = (spv2 - tpv2).Normalized();
            var actualAngle = new Vector2(GlobalTransform.basis.z.x, GlobalTransform.basis.z.z);

            var angleToWantedAngle = actualAngle.Normalized().AngleTo(wantedAngle.Normalized());

            //Console.WriteLine($"angleToWantedAngle={angleToWantedAngle} wantedAngle={wantedAngle} actualAngle={actualAngle}");

            CurrentTurn = 0;
            if (angleToWantedAngle > 5.8f * delta) CurrentTurn = 1;
            if (angleToWantedAngle < -5.8f * delta) CurrentTurn = -1;
        } else {
            Console.WriteLine("RID=NONE");
        }
    }
}
