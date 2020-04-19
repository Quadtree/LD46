using Godot;
using System;

public class BaseCargo
{
    public float HP = 1f;

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