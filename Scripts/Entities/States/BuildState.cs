using Godot;

public class BuildState : IState
{
    Vector2 Location;
    Building Building;
    BuildJob Job;

    float BuildTime;
    bool FrameWait = true;

    public BuildState(Vector2 buildLocation)
    {
        Location = buildLocation;
    }

    public void Start(Character target)
    {
        Building = WorldManager.World.CreateBuilding();
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
                }
                else
                {
                    Building.Position += new Vector2(10, 0);
                }
                break;

            case BuildJob.Building:
                BuildTime += delta;
                if (BuildTime >= 2)
                    target.FinishState();
                break;
        }
    }

    public void End(Character target)
    {
    }

    public string GetDebugInfo()
    {
        return Job.ToString();
    }

    enum BuildJob
    {
        FindingLocation,
        Building
    }
}
