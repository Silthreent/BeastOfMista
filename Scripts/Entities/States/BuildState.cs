using Godot;

public class BuildState : IState
{
    Vector2 Location;
    Building Building;
    BuildJob Job;

    bool FrameWait = true;

    public BuildState(Vector2 buildLocation, Building building)
    {
        Location = buildLocation;
        Building = building;
    }

    public void Start(Character target)
    {
        Building.GlobalPosition = Location;
    }

    public void Process(Character target, float delta)
    {
        if (target.Stats[Stat.Energy] == 0)
        {
            target.AI.SetState(new RelaxState(WorldManager.World.GetBuilding(x => x.BuildType is TownHall)));
            target.AI.FutureState(this);
            return;
        }

        if (FrameWait)
        {
            FrameWait = false;
            return;
        }

        switch (Job)
        {
            case BuildJob.FindingLocation:
                if (Building.InteractArea.GetOverlappingAreas().Count == 1)
                {
                    Job = BuildJob.Building;
                    WorldManager.World.RegisterBuilding(Building);
                }
                else
                {
                    Location = target.GlobalPosition + new Vector2(target.RNG.Next(-50, 50), target.RNG.Next(-50, 50));
                    target.AI.InterruptState(new MovingState(Location));
                    FrameWait = true;
                }
                break;

            case BuildJob.Building:
                if(target.GlobalPosition.DistanceTo(Building.GlobalPosition) > 10)
                {
                    target.AI.InterruptState(new MovingState(Building.GlobalPosition));
                }
                else
                {
                    Building.ProgressProgress(delta);
                    target.Stats.ReduceStat(Stat.Energy, 5 * delta);

                    if (Building.IsCompleted)
                    {
                        target.AI.FinishState();
                    }
                }
                break;
        }
    }

    public void End(Character target)
    {
    }

    public string GetDebugInfo()
    {
        return Job.ToString() + "|" + Building.BuildProgress.ToString("#.##") + "/" + Building.BuildType.MaxBuildProgress;
    }

    enum BuildJob
    {
        FindingLocation,
        Building
    }
}
