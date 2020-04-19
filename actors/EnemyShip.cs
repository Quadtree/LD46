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

    [Export]
    bool IsRamShip = false;

    int CurrentTurn;

    int thrust = 0;

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
            var stations = GetTree().Root.FindChildrenByType<SpaceStation>();
            if (stations.Count == 0) return;

            var closestStation = stations.Min(it => it.GetGlobalLocation().DistanceTo(ps.GetGlobalLocation()));

            var fireIfInRange = false;

            //Console.WriteLine($"closestStation={closestStation}");

            var rangeToTarget = ps.GetGlobalLocation().DistanceTo(this.GetGlobalLocation());

            var targetWithPrediction = ps.GetGlobalLocation() + ps.LinearVelocity * (rangeToTarget / 50);

            if (closestStation < 30)
            {
                var posDelta = this.GetGlobalLocation() - ps.GetGlobalLocation();
                Dest = this.GetGlobalLocation() + posDelta.Normalized() * 100;
            } else {
                Dest = targetWithPrediction;
                fireIfInRange = true;
            }

            if (fireIfInRange && rangeToTarget < 30 && !IsRamShip)
            {
                Console.WriteLine("FIRE");

                if (FireCharge > 0.25f)
                {
                    FireCharge = 0;
                    var proj = (EnemyProjectile)ProjectileType.Instance();
                    GetTree().Root.AddChild(proj);
                    proj.SetGlobalLocation(this.GetGlobalLocation());
                    proj.LookAt(targetWithPrediction, Vector3.Up);
                    proj.LinearVelocity = -proj.Transform.basis.z * 50;

                    Util.SpawnOneShotSound("res://sounds/EnemyShoot.wav", this, this.GetGlobalLocation());
                }
            }

            var likelyArrivalLocation = LinearVelocity.Normalized() * rangeToTarget + this.GetGlobalLocation();

            if (likelyArrivalLocation.DistanceTo(Dest) < 15 || LinearVelocity.Length() < 5 || !fireIfInRange)
            {
                thrust = 1;
            } else {
                thrust = -1;
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

        if (thrust == 1)
        {
            state.AddCentralForce(-state.Transform.basis.z * ThrustPower);
            this.FindChildByName<CPUParticles>("MainEF").Emitting = true;
        } else {
            this.FindChildByName<CPUParticles>("MainEF").Emitting = false;
        }

        if (thrust == -1)
        {
            state.AddCentralForce(state.LinearVelocity.Normalized() * -ThrustPower);
        }
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
