using Godot;
using System;
using System.Linq;

public class PlayerShip : RigidBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public float TurnRate = 1.5f;

    [Export]
    public float ThrustPower = 8f;

    [Export]
    public PackedScene RespawnerType;

    int CurrentTurn;

    private Vector2 OldVelocityV2;

    public BaseCargo Cargo = null;

    public BaseCargo NextCargo = null;

    public SpaceStation NextStation = null;

    public float HP = 1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("body_entered", this, nameof(onBodyEntered));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Cargo != null && Cargo.HP < 0) Cargo = null;

        if (Cargo == null && NextCargo == null)
        {
            ChooseNextStation();

            switch(Util.RandInt(0, 1))
            {
                case 0: NextCargo = new FragileFlatware(); break;
            }
        }
    }

    private void ChooseNextStation()
    {
        var stations = GetTree().Root.FindChildrenByType<SpaceStation>().Where(it => it != NextStation).ToList();
        NextStation = stations[Util.RandInt(0, stations.Count)];
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

    public void onBodyEntered(Node body)
    {
        //Console.WriteLine($"COL {body}");

        if (body == NextStation)
        {
            if (NextCargo != null)
            {
                Cargo = NextCargo;
                NextCargo = null;

                ChooseNextStation();
            }
            else if (Cargo != null)
            {
                Cargo = null;
                // @TODO: mission completion reward
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Console.WriteLine($"Player took {damage} damage");
        HP -= damage;

        if (HP <= 0)
        {
            QueueFree();

            GetTree().Root.AddChild(RespawnerType.Instance());
        }
    }
}
