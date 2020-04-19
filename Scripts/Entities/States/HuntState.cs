using Godot;

public class HuntState : IState
{
    public float RequiredHunger { get; set; }

    Vector2[] CurrentPath;
    int PathMarker;

    public void Start(Character target)
    {
        FindHunt(target);
    }

    public void Process(Character target, float delta)
    {
        if ((target.AI as BeastAI).Hunger >= 100)
        {
            target.AI.FinishState();
            return;
        }

        var movingDir = CurrentPath[PathMarker] - target.GlobalPosition;
        target.Position += movingDir.Normalized() * (target.MovementSpeed * delta);

        if (target.Position.DistanceTo(CurrentPath[PathMarker]) <= 5)
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
    }

    public string GetDebugInfo()
    {
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
