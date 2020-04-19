using Godot;
using System;

public class FragileFlatware : BaseCargo
{
    public override string Name { get { return "Fragile Flatware"; } }

    public override void ChangeInVelocity(float amountPerSecond)
    {
        if (amountPerSecond > 50)
        {
            HP -= amountPerSecond / 1000;
        }
    }
}