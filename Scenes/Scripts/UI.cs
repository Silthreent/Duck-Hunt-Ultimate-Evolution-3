using Godot;

public class UI : MarginContainer
{
    Gameplay game;
    TextureProgress tempBar;

    public override void _Ready()
    {
        game = GetParent().GetParent() as Gameplay;
        tempBar = GetNode("TempBar") as TextureProgress;
    }

    public override void _Process(float delta)
    {
        if(game.CheckHeat() == 0)
        {
            tempBar.Value = 50;
        }
        else if(game.CheckHeat() > 0)
        {
            tempBar.Value = game.CheckHeat() / 2 + 50;
        }
        else
        {
            tempBar.Value = (game.CheckCold() / 2 - 50) * -1;
        }
    }
}