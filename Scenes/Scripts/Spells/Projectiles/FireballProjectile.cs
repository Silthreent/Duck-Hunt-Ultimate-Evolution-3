using Godot;

public class FireballProjectile : RigidBody2D
{
    public int Damage { get; set; }

    public override void _PhysicsProcess(float delta)
    {
        var dir = LinearVelocity.Normalized().Angle();
        SetRotation(dir);
    }

    private void _on_Projectile_body_entered(Object body)
    {
        var area = GetNode("Explosion") as Area2D;
        var bodies = area.GetOverlappingBodies();
        if(bodies.Length > 1)
        {
            foreach(object x in bodies)
            {
                if((x as Node2D).HasUserSignal("takeDamage"))
                {
                    (x as Node2D).EmitSignal("takeDamage", Damage);
                }
            }
        }

        QueueFree();
    }
}