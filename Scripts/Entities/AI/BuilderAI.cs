using Godot;

public class BuilderAI : BaseAI
{
    const int BUILDING_RNG_MODIFIER = 3;

    public BuilderAI(Character owner) : base(owner)
    {
    }

    public override void Process(float delta)
    {
        base.Process(delta);

        if(CurrentState is IdleState)
        {
            if (Owner.CurrentLocation != null)
            {
                if(Owner.CurrentLocation.BuildType is TownHall)
                {
                    if(Owner.Stats[Stat.Energy] < 100)
                    {
                        SetState(new RelaxState(Owner.CurrentLocation));
                    }
                    else
                    {
                        BuildProject();
                    }
                }
                else
                {
                    SetState(new MovingState(WorldManager.World.GetTownHall()));
                }
            }
            else
            {
                BuildProject();
            }
        }
    }

    void BuildProject()
    {
        var newBuild = WorldManager.World.GetNextBuildProject();
        if (newBuild != null)
        {
            if(!newBuild.IsCompleted)
            {
                newBuild.AssignedBuilder = Owner;

                var searchLocation = Owner.GlobalPosition + new Vector2(Owner.RNG.Next(
                    -75 * (WorldManager.World.GetBuildingCount() / BUILDING_RNG_MODIFIER), 75 * (WorldManager.World.GetBuildingCount() / BUILDING_RNG_MODIFIER)),
                    Owner.RNG.Next(-75 * (WorldManager.World.GetBuildingCount() / BUILDING_RNG_MODIFIER), 75 * (WorldManager.World.GetBuildingCount() / BUILDING_RNG_MODIFIER)));
                searchLocation = WorldManager.World.NavMesh.GetClosestPoint(searchLocation);

                GD.Print($"Building new at {searchLocation}");
                SetState(new MovingState(searchLocation));
                NextState = new BuildState(searchLocation, newBuild);
            }
            else
            {
                newBuild.AssignedBuilder = Owner;

                SetState(new RepairState(newBuild));
            }
        }
    }
}
