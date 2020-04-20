using Godot;

public class HuntState : IState
{
    public float RequiredHunger { get; set; }

    Vector2[] CurrentPath;
    int PathMarker;

    public void Start(Character target)
    {
        RequiredHunger = 100 - (((target.AI as BeastAI).Hunger - 100) / 2);
        FindHunt(target);
    }

    public void Process(Character target, float delta)
    {
        if ((target.AI as BeastAI).Hunger >= 100)
        {
            target.AI.SetState(new MovingState(WorldManager.World.BeastCave));
            return;
        }

        var movingDir = CurrentPath[PathMarker] - target.GlobalPosition;
        target.GlobalPosition += movingDir.Normalized() * (target.MovementSpeed * delta);

        if (target.GlobalPosition.DistanceTo(CurrentPath[PathMarker]) <= 5)
        {
            PathMarker++;
            if (PathMarker >= CurrentPath.Length)
                CurrentPath = null;
        }

        if(CurrentPath == null)
        {
            FindHunt(target);
        }
    }

    public void End(Character target)
    {
        WorldManager.World.TriggerEndCheck();
    }

    public string GetDebugInfo()
    {
        if (CurrentPath != null)
            return CurrentPath[PathMarker].ToString();
        else
            return "";
    }

    void FindHunt(Character target)
    {
        if (target.SenseDistance.GetOverlappingAreas() != null)
        {
            foreach (var x in target.SenseDistance.GetOverlappingAreas())
            {
                if (x is Character)
                {
                    if (x != target)
                    {
                        CurrentPath = WorldManager.World.NavMesh.GetSimplePath(target.GlobalPosition, (x as Node2D).GlobalPosition);
                        return;
                    }
                }
            }
        }

        if(WorldManager.World.GetVillagerCount() == 0)
        {
            target.AI.FinishState();
            return;
        }

        CurrentPath = WorldManager.World.NavMesh.GetSimplePath(target.GlobalPosition, WorldManager.World.GetTownHall().GlobalPosition);
        PathMarker = 0;
    }
}
