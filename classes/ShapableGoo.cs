using Godot;
using System;

public class ShapableGoo : BaseCargo
{
    public override string Name { get { return "Shapable Goo"; } }

    public override int Value { get { return 600; }}

    public override string Description { get { return "This goo is deformed by any acceleration. Avoid acceleration."; }}

    public override void ChangeInVelocity(float amountPerSecond)
    {
        HP -= amountPerSecond / 8000;
    }
}