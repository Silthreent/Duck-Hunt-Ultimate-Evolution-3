using Godot;

public class WeaponSpawn : Node2D
{
    bool enabled = true;

    private void _on_Area2D_body_entered(Object body)
    {
        if(!enabled)
            return;

        if((body as Node).Name == "Player")
        {
            if(GetNode("Weapon").GetChildren().Length > 0)
            {
                var gift = GetNode("Weapon").GetChild(0);
                GetNode("Weapon").RemoveChild(gift);

                var spell = (body as Player).GetSpell(gift as Spell);
                if(spell != null)
                {
                    GetNode("Weapon").AddChild(spell);
                }
            }
        }
    }

    public void Disable()
    {
        enabled = false;
        (GetNode("Weapon") as Node2D).Visible = false;
    }

    public void Enable()
    {
        enabled = true;
        (GetNode("Weapon") as Node2D).Visible = true;
    }
}
