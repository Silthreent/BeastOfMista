public class RelaxState : IState
{
    Building RelaxBuilding;

    public RelaxState(Building building)
    {
        RelaxBuilding = building;
    }

    public void Start(Character target)
    {
    }

    public void Process(Character target, float delta)
    {
        if(target.Stats[Stat.Energy] >= 100)
        {
            target.AI.FinishState();
        }

        if (target.GlobalPosition.DistanceTo(RelaxBuilding.GlobalPosition) > 10)
        {
            target.AI.InterruptState(new MovingState(RelaxBuilding.GlobalPosition));
        }
    }

    public void End(Character target)
    {
    }

    public string GetDebugInfo()
    {
        return "Chillaxin'";
    }
}
