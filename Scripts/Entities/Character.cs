using Godot;

public class Character : Node2D
{
	public float MovementSpeed { get; protected set; } = 150f;

	StateManager State;
	Line2D NavLine;

	public override void _Ready()
	{
		State = new StateManager(this);

		NavLine = FindNode("NavLine") as Line2D;
		NavLine.SetAsToplevel(true);
	}

	public override void _Process(float delta)
	{
		State.Process(delta);
	}

	public void ForceState(IState state)
	{
		State.SetState(state);
	}

	public override void _UnhandledInput(InputEvent input)
	{
		var mouse = input as InputEventMouseButton;
		if (mouse != null)
		{
			if(mouse.ButtonIndex == (int)ButtonList.Left && mouse.Pressed)
			{
				var newBuilding = ResourceLoader.Load<PackedScene>(@"Scenes\World\Building.tscn");
				var building = newBuilding.Instance() as Building;
				building.Position = new Vector2(220, 113);

				WorldManager.World.AddChild(building);
				WorldManager.World.RegisterBuilding(building);

				NavLine.Points = WorldManager.World.NavMesh.GetSimplePath(GlobalPosition, WorldManager.World.GetBuilding().Entrance.GlobalPosition);

				State.SetState(new MovingState(WorldManager.World.GetBuilding().Entrance.GlobalPosition));
			}
		}
	}
}
