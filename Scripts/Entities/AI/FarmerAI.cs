public class FarmerAI : BaseAI
{
    Building AssignedLand;
    
    public FarmerAI(Character owner) : base(owner)
    {
        FindEmptyLand();
    }

    public override void Process(float delta)
    {
        base.Process(delta);

        if (CurrentState is IdleState)
        {
            if(AssignedLand == null)
            {
                FindEmptyLand();
            }
            else
            {
                if(AssignedLand.IsFunctional)
                {
                    SetState(new MovingState(AssignedLand.GlobalPosition));
                    NextState = new FarmingState(AssignedLand);
                }
            }
        }
    }

    void FindEmptyLand()
    {
        AssignedLand = WorldManager.World.GetBuilding(
            x => x.IsCompleted
        && (x.BuildType is Farmland)
        && (x.BuildType as Farmland).AssignedWorker == null);

        if(AssignedLand != null)
            (AssignedLand.BuildType as Farmland).AssignWorker(Owner);
    }
}
