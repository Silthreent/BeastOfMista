using Godot;

public class Building : Node2D
{
	public Node2D Entrance { get; protected set; }
	public Polygon2D Collider;

	public override void _Ready()
	{
		Entrance = FindNode("Entrance") as Node2D;
		Collider = FindNode("Collider") as Polygon2D;
	}

	public override void _UnhandledKeyInput(InputEventKey input)
	{
		if(input.IsActionPressed("spawn_building"))
		{
			WorldManager.World.RegisterBuilding(this);
		}

		if (input.IsActionPressed("DEBUGtoggle_buildings"))
		{
			Visible = !Visible;
		}
	}
}
