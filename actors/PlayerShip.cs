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

    private Vector2 OldVelocityV2;

    public BaseCargo Cargo = new FragileFlatware();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Cargo.HP < 0) Cargo = null;
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

        if (Input.IsActionPressed("Thrust Brake"))
        {
            state.AddCentralForce(state.LinearVelocity.Normalized() * -ThrustPower);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        var cam = GetViewport().GetCamera();

        var raySrc = cam.ProjectRayOrigin(GetViewport().GetMousePosition());
        var rayNorm = cam.ProjectRayNormal(GetViewport().GetMousePosition());
        var rayTo = raySrc + rayNorm * 100;

        var pos = new Vector3();

        var curPos = GetWorld().DirectSpaceState.IntersectRay(raySrc, rayTo);

        if (curPos.Contains("position"))
        {
            pos = (Vector3)curPos["position"];

            var tpv2 = new Vector2(pos.x, pos.z);
            var spv2 = new Vector2(this.GetGlobalLocation().x, this.GetGlobalLocation().z);

            var wantedAngle = (spv2 - tpv2).Normalized();
            var actualAngle = new Vector2(GlobalTransform.basis.z.x, GlobalTransform.basis.z.z);

            var angleToWantedAngle = actualAngle.Normalized().AngleTo(wantedAngle.Normalized());

            CurrentTurn = 0;
            if (angleToWantedAngle > 5.8f * delta) CurrentTurn = 1;
            if (angleToWantedAngle < -5.8f * delta) CurrentTurn = -1;
        }

        var newVelocityV2 = new Vector2(LinearVelocity.x, LinearVelocity.z);

        if (OldVelocityV2 != null)
        {
            var change = newVelocityV2.DistanceTo(OldVelocityV2) / delta;

            if (Cargo != null)
                Cargo.ChangeInVelocity(change);
        }

        OldVelocityV2 = newVelocityV2;
    }
}
