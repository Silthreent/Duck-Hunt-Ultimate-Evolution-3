using Godot;
using System;

public class DamageText : Node2D
{
    static Random random = new Random();

    public float StartY { get; set; }

    int speed = 25;

    public DamageText()
    {
        StartY = random.Next(-20, -10);
        SetPosition(new Vector2(random.Next(-20, 20), StartY));
    }

    public override void _PhysicsProcess(float delta)
    {
        SetPosition(new Vector2(Position.x, Position.y - (speed * delta)));

        if(Position.y < StartY - 20)
        {
            QueueFree();
        }
    }
}
