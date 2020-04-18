public class StateManager
{
	Character Owner;
    IState CurrentState;

	public StateManager(Character owner)
	{
		Owner = owner;
	}

	public void Process(float delta)
	{
		if (CurrentState != null)
			CurrentState.Process(Owner, delta);
	}

	public void SetState(IState state)
	{
		if (CurrentState != null)
			CurrentState.End(Owner);

		CurrentState = state;
		CurrentState.Start(Owner);
	}
}
