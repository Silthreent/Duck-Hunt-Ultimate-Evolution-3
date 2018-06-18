using Godot;

public class Fireball : Spell
{
    PackedScene projectile;
    int projectileSpeed = 2000;
    int damage = 45;

    public override void _Ready()
    {
        base._Ready();

        projectile = ResourceLoader.Load("Scenes/Spells/Projectiles/FireballProjectile.tscn") as PackedScene;

        AttackDelay = 1.5f;
    }

    public override void UseSpell()
    {
        FireballProjectile newProjectile = projectile.Instance() as FireballProjectile;
        newProjectile.Position = GlobalPosition;

        newProjectile.LinearVelocity = new Vector2(owner.GetForward().Position - Position).Normalized() * projectileSpeed;
        if(game.CheckHeat() > 0)
        {
            newProjectile.Damage = (int)(damage * (1 + (game.CheckHeat() / 100f)));
        }
        else
        {
            newProjectile.Damage = damage;
        }

        // TODO: Increase fireball radius with Heat
        //newProjectile.GetNode("Explosion/ExplosionArea") as CollisionShape2D.Shape as CircleShape2D).Radius = 180 * ((game.CheckHeat() / 100) + 1);

        GetNode("/root/World").AddChild(newProjectile);

        game.AddHeat(5);
    }
}
