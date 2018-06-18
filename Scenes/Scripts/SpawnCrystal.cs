using Godot;
using System;

public class SpawnCrystal : Node2D
{
    Random random = new Random();
    bool running = false;
    bool completed = false;
    int enemyCount;
    float spawnCounter;
    PackedScene enemy;
    WeaponSpawn weapon;
    object[] spawnPoints;
    Player player;

    public override void _Ready()
    {
        player = GetNode("/root/World/Player") as Player;
        enemy = ResourceLoader.Load("Scenes/Enemy.tscn") as PackedScene;
        weapon = GetNode("WeaponSpawn") as WeaponSpawn;

        if(weapon != null)
        {
            weapon.Disable();
        }

        AddUserSignal("spawnerComplete");
        Connect("spawnerComplete", GetNode("/root/World"), "OnSpawnerComplete");

        foreach(Node x in GetChildren())
        {
            if(x.Name[0] == 'L')
            {
                RemoveChild(x);
                GetNode("SpawnPoints").AddChild(x);
            }
        }

        spawnPoints = GetNode("SpawnPoints").GetChildren();
    }

    public override void _PhysicsProcess(float delta)
    {
        if(completed)
            return;

        if(GlobalPosition.DistanceTo(player.GlobalPosition) <= 600 && !running)
        {
            enemyCount = 1;
            running = true;
        }

        if(running)
        {
            spawnCounter -= delta;

            if(spawnCounter <= 0)
            {
                if(GetNode("Enemies").GetChildren().Length <= 50)
                {
                    SpawnEnemy();
                    while(random.Next(0, 5) < 2)
                    {
                        SpawnEnemy();
                    }

                    spawnCounter = 2;
                }
            }
        }
    }

    void SpawnEnemy()
    {
        var newEnemy = enemy.Instance() as Enemy;
        newEnemy.Position = (spawnPoints[random.Next(0, spawnPoints.Length)] as Node2D).Position;
        newEnemy.AddUserSignal("died");
        newEnemy.Connect("died", this, "OnEnemySpawnDied");
        GetNode("Enemies").AddChild(newEnemy);
    }

    void OnEnemySpawnDied()
    {
        enemyCount--;
        if(enemyCount <= 0 && !completed)
        {
            GetNode("Crystal").QueueFree();
            completed = true;
            if(weapon != null)
                weapon.Enable();
            EmitSignal("spawnerComplete");
        }
    }
}
