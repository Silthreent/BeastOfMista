using Godot;

public class BuildState : IState
{
    Vector2 Location;
    Building Building;
    BuildJob Job;

    float BuildTime;
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
        if (target.Stats.GetStat(Stat.Energy) == 0)
        {
            target.AI.InterruptState(new MovingState(WorldManager.World.GetBuilding(x => x.BuildType is TownHall)));
            target.AI.FutureState(new RelaxState());
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
                    GD.Print("GOOD LOCATION");
                    Job = BuildJob.Building;
                    WorldManager.World.RegisterBuilding(Building);
                }
                else
                {
                    GD.Print("INVALID; FINDING NEW LOCATION");
                    Location = target.GlobalPosition + new Vector2(target.RNG.Next(25, 100), target.RNG.Next(25, 100));
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
                    BuildTime += delta;
                    target.Stats.ReduceStat(Stat.Energy, 5 * delta);
                    if (BuildTime >= 2)
                    {
                        Building.CompleteBuilding();
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
        return Job.ToString() + "|" + BuildTime.ToString("#.##");
    }

    enum BuildJob
    {
        FindingLocation,
        Building
    }
}
