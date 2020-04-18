using Godot;

public class Building : Node2D
{
	public Node2D Entrance { get; protected set; }
	public Polygon2D PathCollider { get; protected set; }
	public Area2D InteractArea { get; protected set; }

	public override void _Ready()
	{
		Entrance = FindNode("Entrance") as Node2D;
		PathCollider = FindNode("PathCollider") as Polygon2D;
		InteractArea = FindNode("InteractArea") as Area2D;
	}

	public override void _UnhandledKeyInput(InputEventKey input)
	{
		if (input.IsActionPressed("DEBUGtoggle_buildings"))
		{
			Visible = !Visible;
		}
	}
}
