using Godot;

public class Building : Node2D
{
	public Node2D Entrance { get; protected set; }
	public Polygon2D PathCollider { get; protected set; }
	public Area2D InteractArea { get; protected set; }
	public BuildingType BuildType { get; protected set; }

	Sprite Sprite;

	public override void _Ready()
	{
		Entrance = FindNode("Entrance") as Node2D;
		PathCollider = FindNode("PathCollider") as Polygon2D;
		InteractArea = FindNode("InteractArea") as Area2D;

		Sprite = FindNode("Sprite") as Sprite;
	}

	public override void _UnhandledKeyInput(InputEventKey input)
	{
		if (input.IsActionPressed("DEBUGtoggle_buildings"))
		{
			Visible = !Visible;
		}
	}

	public void CompleteBuilding()
	{
		Sprite.Visible = true;
	}

	public void SetBuildingType(BuildingType type)
	{
		BuildType = type;

		Sprite.Texture = ResourceLoader.Load<Texture>($@"Assets\Sprites\Buildings\{type.SpriteLocation}.png");
	}
}
