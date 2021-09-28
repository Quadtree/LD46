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

    [Export]
    public int MoneyToRespawn = 500;

    [Export]
    public int MoneyToWin = 1000;

    int CurrentTurn;

    private Vector2 OldVelocityV2;

    public BaseCargo Cargo = null;

    public BaseCargo NextCargo = null;

    public SpaceStation NextStation = null;

    public float CargoInvulnerableTime = 0;

    public float HP = 1f;

    public Vector3 OldZBasis = Vector3.Forward;

    [Export]
    public PackedScene EngineFlare;

    public CPUParticles MainEF;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("body_entered", this, nameof(onBodyEntered));
    }

    public override void _Process(float delta)
    {
        //Util.SpawnOneShotSound((AudioStreamSample)GD.Load("res://sounds/CargoDamaged.wav"), this, this.GetGlobalLocation());

        CargoInvulnerableTime -= delta;
        if (CargoInvulnerableTime > 0 && Cargo != null) Cargo.HP = 1f;

        this.FindChildByName<MeshInstance>("Crate").Visible = Cargo != null;
        Mass = Cargo != null ? 2 : 1;

        if (Cargo != null && Cargo.HP < 0)
        {
            Cargo = null;
            Util.SpawnOneShotSound("res://sounds/CargoDestroyed.wav", this, this.GetGlobalLocation());
        }

        if (Cargo == null && NextCargo == null)
        {
            ChooseNextStation();

            int n = Util.RandInt(0, 3);

            Console.WriteLine($"GetTree().Root = {GetTree().Root}");

            int money = GetTree().Root.FindChildByType<PlayerMoneyHolder>().Money;

            if (money > 2200){
                n = Util.RandInt(3, 8);
            } else if (money > 1000){
                n = Util.RandInt(1, 6);
            }

            //n = Util.RandInt(0, 8);

            switch(n)
            {
                case 0: NextCargo = new BoringCrate(); break;
                case 1: NextCargo = new FragileFlatware(); break;
                case 2: NextCargo = new FreshMeat(); break;

                case 3: NextCargo = new ShapableGoo(); break;
                case 4: NextCargo = new SpeedRat(); break;
                case 5: NextCargo = new UnstableSoufle(); break;

                case 6: NextCargo = new FearfulFerret(); break;
                case 7: NextCargo = new DizzyDoormouse(); break;
            }

            NextCargo.OwningNode = this;
        }

        if (this.GetGlobalLocation().Length() > 3000)
        {
            this.SetGlobalLocation(this.GetGlobalLocation() * -0.8f);
        }

        if (Input.IsActionJustPressed("Take Screenshot"))
        {
            var image = GetViewport().GetTexture().GetData();
            image.FlipY();
            image.SavePng($"user://screenshot{DateTime.Now.ToString().Replace('/', '_').Replace(':', '_').Replace(' ', '_')}.png");

            //OS.DumpMemoryToFile($"C:/tmp/mem_{DateTime.Now.ToString().Replace('/', '_').Replace(':', '_').Replace(' ', '_')}.txt");

            //OS.GetDynamicMemoryUsage();
        }

        Util.SpeedUpPhysicsIfNeeded();
    }

    private void ChooseNextStation()
    {
        var stations = GetTree().Root.FindChildrenByType<SpaceStation>().Where(it => it != NextStation).ToList();
        if (stations.Count > 0)
            NextStation = stations[Util.RandInt(0, stations.Count)];
        else
            NextStation = null;
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
            this.FindChildByName<CPUParticles>("MainEF").Emitting = true;
        } else {
            this.FindChildByName<CPUParticles>("MainEF").Emitting = false;
        }

        if (Input.IsActionPressed("Strafe Left"))
        {
            state.AddCentralForce(-state.Transform.basis.x * ThrustPower);
            this.FindChildByName<CPUParticles>("RightEF").Emitting = true;
        } else {
            this.FindChildByName<CPUParticles>("RightEF").Emitting = false;
        }

        if (Input.IsActionPressed("Strafe Right"))
        {
            state.AddCentralForce(state.Transform.basis.x * ThrustPower);
            this.FindChildByName<CPUParticles>("LeftEF").Emitting = true;
        } else {
            this.FindChildByName<CPUParticles>("LeftEF").Emitting = false;
        }

        if (Input.IsActionPressed("Thrust Brake"))
        {
            state.AddCentralForce(state.LinearVelocity.Normalized() * -ThrustPower);
        }

        if (OS.IsDebugBuild() && Input.IsActionPressed("self_destruct"))
        {
            QueueFree();

            GetTree().Root.AddChild(RespawnerType.Instance());
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

        if (Cargo != null)
        {
            Cargo.Update(LinearVelocity.Length(), delta);

            float amountTurned = Transform.basis.z.AngleTo(OldZBasis);

            Cargo.Turned(amountTurned);

            OldZBasis = Transform.basis.z;
        }

        OldVelocityV2 = newVelocityV2;
    }

    public void onBodyEntered(Node body)
    {
        //Console.WriteLine($"COL {body}");

        if (body is SpaceStation)
        {
            float maxRepairAmount = GetTree().Root.FindChildByType<PlayerMoneyHolder>().Money / 200;

            float actualRepairAmount = Math.Min(maxRepairAmount, 1 - HP);

            HP += actualRepairAmount;
            GetTree().Root.FindChildByType<PlayerMoneyHolder>().Money -= (int)(actualRepairAmount * 200);
        }

        if (body == NextStation)
        {
            if (NextCargo != null)
            {
                Util.SpawnOneShotSound("res://sounds/MissionStart.wav", this, this.GetGlobalLocation());

                Cargo = NextCargo;
                NextCargo = null;

                CargoInvulnerableTime = 0.25f;

                ChooseNextStation();
            }
            else if (Cargo != null)
            {
                Util.SpawnOneShotSound("res://sounds/MissionComplete.wav", this, this.GetGlobalLocation());

                var pmh = GetTree().Root.FindChildByType<PlayerMoneyHolder>();
                pmh.Money += Cargo.Value;

                if (pmh.Money >= MoneyToWin)
                {
                    GetTree().ClearAndChangeScene("res://maps/WinScreen.tscn");
                }

                Cargo = null;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Console.WriteLine($"Player took {damage} damage");
        HP -= damage;

        if (Cargo != null) Cargo.ShipTookDamage(damage);

        if (HP <= 0.01f)
        {
            QueueFree();

            var ps = (PackedScene)GD.Load("res://particles/PlayerShipExplosion.tscn");

            Util.SpawnOneShotCPUParticleSystem(ps, this, this.GetGlobalLocation());

            var pmh = GetTree().Root.FindChildByType<PlayerMoneyHolder>();

            if (pmh.Money >= MoneyToRespawn)
            {
                pmh.Money -= MoneyToRespawn;
                GetTree().Root.AddChild(RespawnerType.Instance());
            }
            else
            {
                GetTree().ClearAndChangeScene("res://maps/LoseScreen.tscn");
            }

            Util.SpawnOneShotSound("res://sounds/ShipDestroyed.wav", this, this.GetGlobalLocation());

        } else {
            Util.SpawnOneShotSound("res://sounds/ShipDamaged.wav", this, this.GetGlobalLocation());
        }
    }
}
