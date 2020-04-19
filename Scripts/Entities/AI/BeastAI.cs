using Godot;

public class BeastAI : BaseAI
{
    public float Hunger { get; protected set; } = 99;

    public BeastAI(Character owner) : base(owner)
    {
    }

    public override void CheckCollision(Area2D area)
    {
        if(area is Character)
        {
            (area as Character).Kill();
            Hunger += 5;
        }
    }
}
