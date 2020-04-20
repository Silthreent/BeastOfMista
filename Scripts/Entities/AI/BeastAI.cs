using Godot;

public class BeastAI : BaseAI
{
    public float Hunger { get; protected set; } = 100;

    public BeastAI(Character owner) : base(owner)
    {
    }

    public override void Process(float delta)
    {
        base.Process(delta);

        Hunger -= .5f * delta;

        if(Hunger <= 0)
        {
            SetState(new HuntState());
        }
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
