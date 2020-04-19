using Godot;

public class BuilderAI : BaseAI
{
    public BuilderAI(Character owner) : base(owner)
    {
    }

    public override void Process(float delta)
    {
        base.Process(delta);

        if(CurrentState is IdleState)
        {
            var searchLocation = Owner.GlobalPosition + new Vector2(Owner.RNG.Next(-200, 200), Owner.RNG.Next(-200, 200));
            searchLocation = WorldManager.World.NavMesh.GetClosestPoint(searchLocation);

            GD.Print($"Building at {searchLocation}");

            SetState(new MovingState(searchLocation));
            NextState = new BuildState(searchLocation);
        }
    }
}
