using Godot;
using System;

public class FreshMeat : BaseCargo
{
    public override string Name { get { return "Fresh Meat"; } }

    public override int Value { get { return 400; }}

    public override string Description { get { return "This meat is extremely fresh. Deliver it before it rots."; }}

    public override void Update(float speed, float delta)
    {
        HP -= delta / 40;
    }
}