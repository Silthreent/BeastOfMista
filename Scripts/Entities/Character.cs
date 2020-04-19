using Godot;
using System;
using System.Text;

public class Character : Area2D
{
	public float MovementSpeed { get; protected set; } = 150f;
	public BaseAI AI { get; protected set; }
	public Random RNG { get; protected set; }
	public Building CurrentLocation { get; set; }
	public StatManager Stats { get; protected set; }
	public InventoryManager Inventory { get; protected set; }

	Label DebugStateLabel;
	Label DebugStatLabel;

	public Character()
	{
		RNG = new Random();
		Stats = new StatManager();
		Inventory = new InventoryManager();

		AI = new BuilderAI(this);
	}

	public override void _Ready()
	{
		DebugStateLabel = FindNode("DebugLabel") as Label;
		DebugStatLabel = FindNode("StatLabel") as Label;
	}

	public override void _Process(float delta)
	{
		AI.Process(delta);
		DebugStateLabel.Text = AI.CurrentState.GetType().ToString() + "\n" + AI.CurrentState.GetDebugInfo();

		StringBuilder builder = new StringBuilder();
		foreach(Stat x in Enum.GetValues(typeof(Stat)))
		{
			builder.Append(x.ToString() + ": ");
			builder.Append(Stats[x].ToString("#.##"));
			builder.Append('\n');
		}
		DebugStatLabel.Text = builder.ToString();
	}

	public void SetJob(BaseAI job)
	{
		AI = job;
	}
}
