using Godot;
using System;

public class DizzyDoormouse : BaseCargo
{
    public override string Name { get { return "Dizzy Doormouse"; } }

    public override int Value { get { return 800; }}

    public override string Description { get { return "This mouse becomes dizzy if you turn too much. Avoid turning."; }}

    public override void ChangeInVelocity(float amountPerSecond)
    {
        if (amountPerSecond > 600)
        {
            HP -= amountPerSecond / 2000;
        }
    }

    public override void Turned(float amount)
    {
        HP -= amount / 15;
    }
}