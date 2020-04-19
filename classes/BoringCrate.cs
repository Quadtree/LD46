using Godot;
using System;

public class BoringCrate : BaseCargo
{
    public override string Name { get { return "Boring Crate"; } }

    public override int Value { get { return 150; }}

    public override string Description { get { return "This crate is boring. Just avoid crashing really hard."; }}

    public override void ChangeInVelocity(float amountPerSecond)
    {
        if (amountPerSecond > 750)
        {
            HP -= amountPerSecond / 3000;
        }
    }
}