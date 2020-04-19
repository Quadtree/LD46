using Godot;
using System;

public class FearfulFerret : BaseCargo
{
    public override string Name { get { return "Fearful Ferret"; } }

    public override int Value { get { return 1000; }}

    public override string Description { get { return "This ferret may be scared to death if you take too much damage. It also dislikes high speed. Avoid damage and speed."; }}

    public override void ChangeInVelocity(float amountPerSecond)
    {
        if (amountPerSecond > 600)
        {
            HP -= amountPerSecond / 2000;
        }
    }

    public override void ShipTookDamage(float amount)
    {
        HP -= amount * 2;
    }

    public override void Update(float speed, float delta)
    {
        if (speed > 50)
        {
            HP -= delta / 5;
        }
    }
}