using Godot;

public class BaseAI
{
    public IState CurrentState { get; protected set; }

    protected Character Owner;

    public BaseAI(Character owner)
    {
        Owner = owner;

        CurrentState = new IdleState();
    }

    public virtual void Process(float delta)
    {
        CurrentState.Process(Owner, delta);
    }

    public void SetState(IState state)
    {
        GD.Print($"Set state to: {state}");
        if(CurrentState != null)
        {
            CurrentState.End(Owner);
        }

        CurrentState = state;
        CurrentState.Start(Owner);
    }

    public void FinishState()
    {
        CurrentState.End(Owner);

        GD.Print($"Last state finished set to idle");
        CurrentState = new IdleState();
    }
}
