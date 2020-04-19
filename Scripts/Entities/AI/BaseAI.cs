using Godot;

public class BaseAI
{
    public IState CurrentState { get; protected set; }
    public IState NextState { get; set; }
    public IState PreviousState { get; set; }

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

    public void InterruptState(IState state)
    {
        PreviousState = CurrentState;

        SetState(state);
    }

    public void FinishState()
    {
        CurrentState.End(Owner);

        if(NextState != null)
        {
            SetState(NextState);
            NextState = null;
        }
        else if(PreviousState != null)
        {
            SetState(PreviousState);
            PreviousState = null;
        }
        else
        {
            SetState(new IdleState());
        }
    }
}
