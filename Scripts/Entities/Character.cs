using Godot;
using System;
using System.Text;

public class Character : Area2D
{
	[Signal]
	public delegate void VillagerDied(Character villager);

	public float MovementSpeed { get; set; } = 150f;
	public BaseAI AI { get; protected set; }
	public Random RNG { get; protected set; }
	public Building CurrentLocation { get; set; }
	public StatManager Stats { get; protected set; }
	public InventoryManager Inventory { get; protected set; }
	public Area2D SenseDistance { get; protected set; }

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

		SenseDistance = FindNode("SenseDistance") as Area2D;

		Connect("area_entered", this, "OnAreaEntered");
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

	public void SetSprite(string name)
	{
		GetNode<Sprite>("Sprite").Texture = ResourceLoader.Load<Texture>($@"Assets\Sprites\Characters\{name}.png");
	}

	public void SetJob(BaseAI job)
	{
		AI = job;
	}

	public void Kill()
	{
		EmitSignal("VillagerDied", this);

		QueueFree();
	}

	void OnAreaEntered(Area2D area)
	{
		AI.CheckCollision(area);
	}
}
