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
        if (target.Stats.GetStat(Stat.Energy) == 0)
        {
            target.AI.InterruptState(new MovingState(WorldManager.World.GetBuilding(x => x.BuildType is TownHall)));
            target.AI.FutureState(new RelaxState());
        }

        if (target.GlobalPosition.DistanceTo(WorkingFarmland.GlobalPosition) > 10)
        {
            target.AI.InterruptState(new MovingState(WorkingFarmland.GlobalPosition));
        }
        else
        {
            (WorkingFarmland.BuildType as Farmland).IncreaseStockpile(1 * delta);
            target.Stats.ReduceStat(Stat.Energy, 10 * delta);
        }
    }

    public void End(Character target)
    {
    }

    public string GetDebugInfo()
    {
        return (WorkingFarmland.BuildType as Farmland).WheatStockpile.ToString();
    }
}
