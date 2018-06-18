using Godot;
using System;

public class Player : KinematicBody2D
{
    int health = 150;
    int speed = 700;
    int jumpHeight = 100;
    int gravity = 250;
    int acceleration = 75;
    int maxJumps = 2;
    int jumpCount;
    float attackDelay = 0;
    AnimatedSprite sprite;
    Gameplay game;
    Vector2 velocity;
    Node2D forward;
    Spell equippedSpell;
    PackedScene damageText;
    AnimationPlayer animPlayer;

    public override void _Ready()
    {
        game = GetParent() as Gameplay;
        forward = GetNode("Forward") as Node2D;
        equippedSpell = GetNode("Spells").GetChildren()[0] as Spell;
        sprite = GetNode("Sprite") as AnimatedSprite;
        animPlayer = GetNode("Sprite/Anims") as AnimationPlayer;
        damageText = ResourceLoader.Load("Scenes/DamageText.tscn") as PackedScene;

        AddUserSignal("takeDamage");
        Connect("takeDamage", this, "TakeDamage");
    }

    public override void _PhysicsProcess(float delta)
    {
        #region Movement

        if(IsOnFloor())
        {
            jumpCount = 0;
        }
        else
        {
            velocity.y += gravity * delta;
        }

        if(Input.IsActionJustPressed("move_jump"))
        {
            if(jumpCount < maxJumps)
            {
                velocity.y = -(gravity + jumpHeight);
                jumpCount++;
                if(jumpCount > 1)
                {
                    animPlayer.Play("DoubleJump");
                }
            }
        }

        if(Input.IsActionPressed("move_left"))
        {
            velocity.x = Mathf.Max(-speed, velocity.x - acceleration);
            forward.Position = new Vector2(-90, 0);
            sprite.FlipH = true;

        }
        else if(Input.IsActionPressed("move_right"))
        {
            velocity.x = Mathf.Min(speed, velocity.x + acceleration);
            forward.Position = new Vector2(90, 0);
            sprite.FlipH = false;
        }
        else
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, 2);
        }

        if(velocity.x != 0)
        {
            sprite.Play("walk");
        }
        else
        {
            sprite.Play("idle");
        }

        if(velocity.y < 0)
        {
            sprite.Play("jump");
        }

        MoveAndSlide(velocity, new Vector2(0, -1));
        #endregion Movement

        if(Input.IsActionPressed("use_spell"))
        {
            if(attackDelay <= 0)
            {
                equippedSpell.UseSpell();
                attackDelay = equippedSpell.AttackDelay;
            }
        }
        if(Input.IsActionJustPressed("switch_spell"))
        {
            var spells = GetNode("Spells").GetChildren();
            var currentValue = Array.IndexOf(spells, equippedSpell);
            if(currentValue >= spells.Length - 1)
            {
                equippedSpell.Visible = false;
                equippedSpell = spells[0] as Spell;
                equippedSpell.Visible = true;
            }
            else
            {
                equippedSpell.Visible = false;
                equippedSpell = spells[currentValue + 1] as Spell;
                equippedSpell.Visible = true;
            }
        }

        attackDelay -= delta;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        var dmg = damageText.Instance();
        (dmg.GetNode("Label") as Label).Text = "-" + damage;
        AddChild(dmg);

        if(health <= 0)
        {
            game.LoseGame();
        }
    }

    public Spell GetSpell(Spell spell)
    {
        if(GetNode("Spells").GetChildren().Length >= 3)
        {
            var holder = equippedSpell;
            GetNode("Spells").RemoveChild(spell);
            equippedSpell = spell;
            equippedSpell.SetUp();

            return holder;
        }

        GetNode("Spells").AddChild(spell);
        equippedSpell.Visible = false;
        equippedSpell = spell;
        equippedSpell.SetUp();

        return null;
    }

    public Node2D GetForward()
    {
        return forward;
    }
}
