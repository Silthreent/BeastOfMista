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
        if (FrameWait)
        {
            FrameWait = false;
            return;
        }

        switch (Job)
        {
            case BuildJob.FindingLocation:
                if (Building.InteractArea.GetOverlappingAreas().Count == 0)
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
                BuildTime += delta;
                if (BuildTime >= 2)
                {
                    Building.CompleteBuilding();
                    target.AI.FinishState();
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
