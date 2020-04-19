using Godot;
using System;

public class BaseCargo
{
    private float _HP = 1f;

    public float HP {
        get {
            return _HP;
        }
        set {
            if (value < _HP){
                Util.SpawnOneShotSound((AudioStreamSample)GD.Load("res://sounds/CargoDamaged.wav"), OwningNode, OwningNode.GetGlobalLocation());
            }
            _HP = value;
        }
    }

    public Spatial OwningNode;

    public virtual string Name { get { return "???"; }}

    public virtual int Value { get { return 0; }}

    public virtual string Description { get { return "?DESC?"; }}

    public virtual void ChangeInVelocity(float amountPerSecond)
    {
        if (amountPerSecond > 30)
            Console.WriteLine($"amountPerSecond={amountPerSecond}");
    }

    public virtual void Update(float speed, float delta)
    {

    }

    public virtual void Turned(float amount)
    {

    }

    public virtual void ShipTookDamage(float amount)
    {

    }
}