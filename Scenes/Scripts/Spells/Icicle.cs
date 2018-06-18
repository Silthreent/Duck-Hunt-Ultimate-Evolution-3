using Godot;

public class Icicle : Spell
{
    PackedScene projectile;
    int projectileSpeed = 2500;
    int damage = 15;

    public override void _Ready()
    {
        base._Ready();

        projectile = ResourceLoader.Load("Scenes/Spells/Projectiles/IcicleProjectile.tscn") as PackedScene;

        AttackDelay = .5f;
    }

    public override void UseSpell()
    {
        IcicleProjectile newProjectile = projectile.Instance() as IcicleProjectile;
        newProjectile.Position = GlobalPosition;

        if(game.CheckCold() >= 10)
        {
            newProjectile.LinearVelocity = new Vector2(owner.GetForward().Position - Position).Normalized() * (projectileSpeed * 2);
            newProjectile.Damage = damage * 2;
        }
        else
        {
            newProjectile.LinearVelocity = new Vector2(owner.GetForward().Position - Position).Normalized() * projectileSpeed;
            newProjectile.Damage = damage;
        }

        GetNode("/root/World").AddChild(newProjectile);

        game.AddCold(5);
    }
}
