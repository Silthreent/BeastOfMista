using Godot;

public class Character : Node2D
{
	public float MovementSpeed { get; protected set; } = 150f;

	BaseAI AI;
	Line2D NavLine;
	Label DebugStateLabel;

	public override void _Ready()
	{
		AI = new BuilderAI(this);
		DebugStateLabel = FindNode("DebugLabel") as Label;

		NavLine = FindNode("NavLine") as Line2D;
		NavLine.SetAsToplevel(true);
	}

	public override void _Process(float delta)
	{
		AI.Process(delta);
		DebugStateLabel.Text = AI.CurrentState.GetType().ToString() + "\n" + AI.CurrentState.GetDebugInfo();
	}

	public void FinishState()
	{
		AI.FinishState();
	}

	public void ForceState(IState state)
	{
		AI.SetState(state);
	}
}
