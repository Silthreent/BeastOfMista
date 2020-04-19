public class RelaxState : IState
{

    public void Start(Character target)
    {
    }

    public void Process(Character target, float delta)
    {
        if(target.Stats.GetStat(Stat.Energy) >= 100)
        {
            target.AI.FinishState();
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
