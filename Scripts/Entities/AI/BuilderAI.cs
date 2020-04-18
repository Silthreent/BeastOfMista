using Godot;
using System;

public class BuilderAI : BaseAI
{
    Random RNG;
    bool BuildLocationSet = false;
    Vector2 BuildLocation;

    public BuilderAI(Character owner) : base(owner)
    {
        RNG = new Random();
    }

    public override void Process(float delta)
    {
        base.Process(delta);

        if(CurrentState is IdleState)
        {
            if (!BuildLocationSet)
            {
                SetToBuild();
                BuildLocationSet = true;
            }
            else
            {
                SetState(new BuildState(BuildLocation));
                BuildLocationSet = false;
            }
        }
    }

    void SetToBuild()
    {
        var searchLocation = Owner.GlobalPosition + new Vector2(RNG.Next(-200, 200), RNG.Next(-200, 200));
        BuildLocation = WorldManager.World.NavMesh.GetClosestPoint(searchLocation);

        GD.Print($"Building at {BuildLocation}");
        SetState(new MovingState(BuildLocation));
    }
}
