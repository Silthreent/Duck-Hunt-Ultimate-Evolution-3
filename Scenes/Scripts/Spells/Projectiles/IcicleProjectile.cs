using Godot;

public class IcicleProjectile : RigidBody2D
{
    public int Damage { get; set; }

    public override void _PhysicsProcess(float delta)
    {
        var dir = LinearVelocity.Normalized().Angle();
        SetRotation(dir);
    }

    private void _on_Projectile_body_entered(Object body)
    {
        if(body.HasUserSignal("takeDamage"))
        {
            body.EmitSignal("takeDamage", Damage);
        }

        QueueFree();
    }
}