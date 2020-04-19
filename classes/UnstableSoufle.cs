using Godot;
using System;

public class UnstableSoufle : BaseCargo
{
    public override string Name { get { return "Unstable Soufflé"; } }

    public override int Value { get { return 500; }}

    public override string Description { get { return "This Soufflé might collapse in any impact. Be careful."; }}

    public override void ChangeInVelocity(float amountPerSecond)
    {
        if (amountPerSecond > 50 && Util.random() < 0.2f)
        {
            HP -= 1000;
        }
    }
}