using Godot;
using System;

public class Character : Node2D
{
	public float MovementSpeed { get; protected set; } = 150f;
	public BaseAI AI { get; protected set; }
	public Random RNG { get; protected set; }
	public Building CurrentLocation { get; protected set; }

	Label DebugStateLabel;

	public override void _Ready()
	{
		RNG = new Random();

		AI = new BuilderAI(this);
		DebugStateLabel = FindNode("DebugLabel") as Label;
	}

	public override void _Process(float delta)
	{
		AI.Process(delta);
		DebugStateLabel.Text = AI.CurrentState.GetType().ToString() + "\n" + AI.CurrentState.GetDebugInfo();
	}
}
