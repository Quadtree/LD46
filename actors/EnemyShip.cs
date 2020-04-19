using Godot;
using System;
using System.Linq;

public class EnemyShip : RigidBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public float TurnRate = 1.5f;

    [Export]
    public float ThrustPower = 8f;

    int CurrentTurn;

    Vector3 Dest;

    float FireCharge;

    [Export]
    PackedScene ProjectileType;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        FireCharge += delta;

        var ps = GetTree().Root.FindChildByType<PlayerShip>();

        if (ps != null)
        {
            var closestStation = GetTree().Root.FindChildrenByType<SpaceStation>().Min(it => it.GetGlobalLocation().DistanceTo(ps.GetGlobalLocation()));

            var fireIfInRange = false;

            //Console.WriteLine($"closestStation={closestStation}");

            if (closestStation < 30)
            {
                var posDelta = this.GetGlobalLocation() - ps.GetGlobalLocation();
                Dest = this.GetGlobalLocation() + posDelta.Normalized() * 100;
            } else {
                Dest = ps.GetGlobalLocation();
                fireIfInRange = true;
            }

            var rangeToTarget = ps.GetGlobalLocation().DistanceTo(this.GetGlobalLocation());

            if (fireIfInRange && rangeToTarget < 30)
            {
                Console.WriteLine("FIRE");

                if (FireCharge > 0.25f)
                {
                    FireCharge = 0;
                    var proj = (EnemyProjectile)ProjectileType.Instance();
                    GetTree().Root.AddChild(proj);
                    proj.SetGlobalLocation(this.GetGlobalLocation());
                    proj.LookAt(ps.GetGlobalLocation() + ps.LinearVelocity * (rangeToTarget / 50), Vector3.Up);
                    proj.LinearVelocity = -proj.Transform.basis.z * 50;
                }
            }

            if (rangeToTarget > 200)
            {
                QueueFree();
            }
        }
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

        if (true)
        {
            state.AddCentralForce(-state.Transform.basis.z * ThrustPower);
        }

        /*if (Input.IsActionPressed("Thrust Brake"))
        {
            state.AddCentralForce(state.LinearVelocity.Normalized() * -ThrustPower);
        }*/
    }

    public override void _PhysicsProcess(float delta)
    {
        var pos = Dest;

        var tpv2 = new Vector2(pos.x, pos.z);
        var spv2 = new Vector2(this.GetGlobalLocation().x, this.GetGlobalLocation().z);

        var wantedAngle = (spv2 - tpv2).Normalized();
        var actualAngle = new Vector2(GlobalTransform.basis.z.x, GlobalTransform.basis.z.z);

        var angleToWantedAngle = actualAngle.Normalized().AngleTo(wantedAngle.Normalized());

        CurrentTurn = 0;
        if (angleToWantedAngle > 5.8f * delta) CurrentTurn = 1;
        if (angleToWantedAngle < -5.8f * delta) CurrentTurn = -1;
    }
}
