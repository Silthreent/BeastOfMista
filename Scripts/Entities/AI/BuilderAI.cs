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
            Building newBuilding = WorldManager.World.CreateBuilding();
            if (WorldManager.World.GetBuilding(x => x.BuildType is TownHall) == null)
            {
                newBuilding.SetBuildingType(new TownHall());
            }
            else
            {
                newBuilding.SetBuildingType(new BuildingType());
            }

            var searchLocation = Owner.GlobalPosition + new Vector2(Owner.RNG.Next(-200, 200), Owner.RNG.Next(-200, 200));
            searchLocation = WorldManager.World.NavMesh.GetClosestPoint(searchLocation);

            GD.Print($"Building new at {searchLocation}");

            SetState(new MovingState(searchLocation));
            NextState = new BuildState(searchLocation, newBuilding);
        }
    }
}
