using Godot;
using System;

public class Spell : Node2D
{
    public float AttackDelay { get; protected set; }

    protected Gameplay game;
    protected Player owner;

    public override void _Ready()
    {
        game = GetNode("/root/World") as Gameplay;
        owner = GetParent().GetParent() as Player;
    }

    public virtual void SetUp()
    {
        owner = GetParent().GetParent() as Player;
    }

    public virtual void UseSpell(){ }
}
