public class FarmingState : IState
{
    Building WorkingFarmland;

    public FarmingState(Building farmland)
    {
        WorkingFarmland = farmland;
    }

    public void Start(Character target)
    {
    }

    public void Process(Character target, float delta)
    {
        if (target.Stats[Stat.Energy] == 0)
        {
            target.Inventory.GainItem(Item.Wheat, WorkingFarmland.Storage[Item.Wheat]);
            WorkingFarmland.Storage.LoseItem(Item.Wheat, WorkingFarmland.Storage[Item.Wheat]);

            target.AI.SetState(new RelaxState(WorldManager.World.GetBuilding(x => x.BuildType is TownHall)));
            target.AI.FutureState(new DropOffState(WorldManager.World.GetBuilding(x => x.BuildType is TownHall), Item.Wheat));
        }

        if (target.GlobalPosition.DistanceTo(WorkingFarmland.GlobalPosition) > 10)
        {
            target.AI.InterruptState(new MovingState(WorkingFarmland.GlobalPosition));
        }
        else
        {
            WorkingFarmland.Storage.GainItem(Item.Wheat, 1 * delta);
            target.Stats.ReduceStat(Stat.Energy, 10 * delta);
        }
    }

    public void End(Character target)
    {
    }

    public string GetDebugInfo()
    {
        return WorkingFarmland.Storage[Item.Wheat].ToString("#.##");
    }
}
