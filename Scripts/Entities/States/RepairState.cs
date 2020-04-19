public class RepairState : IState
{
    Building RepairTarget;

    public RepairState(Building target)
    {
        RepairTarget = target;
    }

    public void Start(Character target)
    {
    }

    public void Process(Character target, float delta)
    {
        if(target.GlobalPosition.DistanceTo(RepairTarget.GlobalPosition) > 10)
        {
            target.AI.InterruptState(new MovingState(RepairTarget));
        }
        else
        {
            RepairTarget.ProgressProgress(delta);
            target.Stats.ReduceStat(Stat.Energy, 5 * delta);

            if (RepairTarget.IsFunctional)
            {
                target.AI.FinishState();
            }
        }
    }

    public void End(Character target)
    {
    }

    public string GetDebugInfo()
    {
        return RepairTarget.BuildProgress + "|" + RepairTarget.BuildType.MaxBuildProgress;
    }
}
