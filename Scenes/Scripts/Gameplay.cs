using Godot;

public class Gameplay : Node
{
    int spawnerCount;
    int tempBar;
    Navigation2D navmesh;
    float timer;

    public override void _Ready()
    {
        navmesh = GetNode("NavMesh") as Navigation2D;
        tempBar = 0;

        spawnerCount = GetNode("Spawners").GetChildren().Length;
        GD.Print(spawnerCount);
    }

    public override void _Process(float delta)
    {
        timer += delta;
    }

    public Vector2[] FindPath(Vector2 start, Vector2 end)
    {
        return navmesh.GetSimplePath(start, end);
    }

    public Vector2[] FindPathToPlayer(Vector2 start)
    {
        return FindPath(start, (GetNode("Player") as Node2D).GlobalPosition);
    }

    public void AddCold(int amount)
    {
        tempBar -= amount;
        if(tempBar < -100)
        {
            tempBar = -100;
        }
    }

    public void AddHeat(int amount)
    {
        tempBar += amount;
        if(tempBar > 100)
        {
            tempBar = 100;
        }
    }

    public int CheckCold()
    {
        return tempBar * -1;
    }

    public int CheckHeat()
    {
        return tempBar;
    }

    public void OnSpawnerComplete()
    {
        spawnerCount--;
        GD.Print(spawnerCount);
        if(spawnerCount <= 0)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        (GetNode("UILayer/UI/Label") as Label).Text = "You win! " + (int)timer + " seconds";
        PauseMode = PauseModeEnum.Stop;
    }

    public void LoseGame()
    {
        (GetNode("UILayer/UI/Label") as Label).Text = "You lose";
        PauseMode = PauseModeEnum.Stop;
    }
}
