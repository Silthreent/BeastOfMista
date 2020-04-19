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
            if (Owner.Inventory[Item.Wheat] >= 300)
            {
                Owner.Inventory.LoseItem(Item.Wheat, 300);

                BuildBuilding();
            }
            else if (Owner.CurrentLocation != null)
            {
                if(Owner.CurrentLocation.BuildType is TownHall)
                {
                    if(Owner.Stats[Stat.Energy] < 100)
                    {
                        SetState(new RelaxState(Owner.CurrentLocation));
                    }
                    else if (Owner.CurrentLocation.Storage[Item.Wheat] >= 300)
                    {
                        Owner.CurrentLocation.Storage.LoseItem(Item.Wheat, 300);

                        BuildBuilding();
                    }
                }
                else
                {
                    SetState(new MovingState(WorldManager.World.GetBuilding(x => x.BuildType is TownHall)));
                }
            }
            else
            {
                SetState(new MovingState(WorldManager.World.GetBuilding(x => x.BuildType is TownHall)));
            }
        }
    }

    void BuildBuilding()
    {
        Building newBuilding = WorldManager.World.CreateBuilding();

        var searchLocation = Owner.GlobalPosition + new Vector2(Owner.RNG.Next(-200, 200), Owner.RNG.Next(-200, 200));
        searchLocation = WorldManager.World.NavMesh.GetClosestPoint(searchLocation);

        GD.Print($"Building new at {searchLocation}");

        SetState(new MovingState(searchLocation));
        NextState = new BuildState(searchLocation, newBuilding);
    }
}
