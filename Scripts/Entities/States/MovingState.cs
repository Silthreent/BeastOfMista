using Godot;

public class MovingState : IState
{
    Vector2 Target;
    Vector2[] CurrentPath;
    int PathMarker;

    public MovingState(Vector2 target)
    {
        Target = target;
    }

    public void Start(Character target)
    {
        CurrentPath = WorldManager.World.NavMesh.GetSimplePath(target.GlobalPosition, Target);
    }

    public void Process(Character target, float delta)
    {
        var movingDir = CurrentPath[PathMarker] - target.GlobalPosition;
        target.Position += movingDir.Normalized() * (target.MovementSpeed * delta);

        if (target.Position.DistanceTo(CurrentPath[PathMarker]) <= 5)
        {
            PathMarker++;
            if (PathMarker >= CurrentPath.Length)
                target.AI.FinishState();
        }
    }

    public void End(Character target)
    {
    }

    public string GetDebugInfo()
    {
        return CurrentPath[PathMarker].ToString();
    }
}

