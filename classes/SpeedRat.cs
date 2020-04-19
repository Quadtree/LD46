using Godot;
using System;

public class SpeedRat : BaseCargo
{
    public override string Name { get { return "Speed Serpent"; } }

    public override int Value { get { return 600; }}

    public override string Description { get { return "Specially trained snake that enjoys speed. Don't go slow or it will die of boredom."; }}

    public override void ChangeInVelocity(float amountPerSecond)
    {
        if (amountPerSecond > 600)
        {
            HP -= amountPerSecond / 2000;
        }
    }

    public override void Update(float speed, float delta)
    {
        if (speed < 20)
        {
            HP -= delta / 5;
        }
    }
}