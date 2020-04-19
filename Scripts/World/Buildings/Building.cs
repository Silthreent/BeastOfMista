using Godot;
using System.Collections.Generic;

public class Building : Node2D
{
	public Node2D Entrance { get; protected set; }
	public Polygon2D PathCollider { get; protected set; }
	public Area2D InteractArea { get; protected set; }
	public BuildingType BuildType { get; protected set; }

	Sprite Sprite;
	List<Character> InsideBuilding;

	public override void _Ready()
	{
		InsideBuilding = new List<Character>();

		Entrance = FindNode("Entrance") as Node2D;
		PathCollider = FindNode("PathCollider") as Polygon2D;
		InteractArea = FindNode("InteractArea") as Area2D;

		Sprite = FindNode("Sprite") as Sprite;
	}

	public override void _Process(float delta)
	{
		foreach(var x in InsideBuilding)
		{
			BuildType.ProcessPatron(x, delta);
		}
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

	public void EnterBuilding(Character character)
	{
		GD.Print($"{character} has entered {BuildType.GetType()}");
		InsideBuilding.Add(character);
		GD.Print(InsideBuilding.Count);

		character.CurrentLocation = this;
	}

	public void ExitBuilding(Character character)
	{
		GD.Print($"{character} has left {BuildType.GetType()}");
		InsideBuilding.Remove(character);
		GD.Print(InsideBuilding.Count);

		if(character.CurrentLocation == this)
			character.CurrentLocation = null;
	}

	public void SetBuildingType(BuildingType type)
	{
		BuildType = type;

		Sprite.Texture = ResourceLoader.Load<Texture>($@"Assets\Sprites\Buildings\{type.SpriteLocation}.png");
	}
}
