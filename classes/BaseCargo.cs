using Godot;
using System;

public class BaseCargo
{
    public float HP = 1f;

    public virtual string Name { get { return "???"; }}

    public virtual int Value { get { return 0; }}

    public virtual void ChangeInVelocity(float amountPerSecond)
    {
        if (amountPerSecond > 30)
            Console.WriteLine($"amountPerSecond={amountPerSecond}");
    }
}