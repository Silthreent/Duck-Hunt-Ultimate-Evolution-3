using Godot;

public class Enemy : KinematicBody2D
{
    int movementSpeed = 500;
    int health = 40;
    PackedScene projectile;
    PackedScene damageText;
    float attackTimer = 1.5f;
    Vector2[] path;

    public override void _Ready()
    {
        damageText = ResourceLoader.Load("Scenes/DamageText.tscn") as PackedScene;
        projectile = ResourceLoader.Load("Scenes/Spells/Projectiles/EnemyProjectile.tscn") as PackedScene;

        AddUserSignal("takeDamage");
        Connect("takeDamage", this, "TakeDamage");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(path == null)
        {
            path = (GetNode("/root/World") as Gameplay).FindPathToPlayer(GlobalPosition);
        }
        else
        {
            if(path.Length < 2)
            {
                path = null;
                return;
            }

            var dir = (path[1] - GlobalPosition).Normalized();
            MoveAndSlide(dir * movementSpeed, new Vector2(0, -1));
            if(GlobalPosition.DistanceTo(path[1]) <= 10)
            {
                path = null;
            }
        }

        if(attackTimer <= 0)
        {
            Attack();
            attackTimer = 1.5f;
        }

        attackTimer -= delta;
    }

    protected virtual void Attack()
    {
        IcicleProjectile newProjectile = projectile.Instance() as IcicleProjectile;
        newProjectile.Position = GlobalPosition;
        newProjectile.Damage = 10;
        newProjectile.LinearVelocity = new Vector2((GetNode("/root/World/Player") as Node2D).GlobalPosition - GlobalPosition).Normalized() * 1250;

        GetNode("/root/World").AddChild(newProjectile);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        var dmg = damageText.Instance();
        (dmg.GetNode("Label") as Label).Text = "-" + damage;
        AddChild(dmg);

        if(health <= 0)
        {
            EmitSignal("died");
            QueueFree();
        }
    }
}
