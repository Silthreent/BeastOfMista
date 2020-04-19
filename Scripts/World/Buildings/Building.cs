using Godot;
using System.Collections.Generic;

public class Building : Node2D
{
	public Polygon2D PathCollider { get; protected set; }
	public Area2D InteractArea { get; protected set; }
	public BuildingType BuildType { get; protected set; }

	Sprite Sprite;
	List<Character> InsideBuilding;

	public override void _Ready()
	{
		InsideBuilding = new List<Character>();

		PathCollider = FindNode("PathCollider") as Polygon2D;

		InteractArea = FindNode("InteractArea") as Area2D;
		InteractArea.Connect("area_entered", this, "OnAreaEntered");
		InteractArea.Connect("area_exited", this, "OnAreaExited");

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

	void EnterBuilding(Character character)
	{
		InsideBuilding.Add(character);

		character.CurrentLocation = this;
	}

	void ExitBuilding(Character character)
	{
		InsideBuilding.Remove(character);

		if(character.CurrentLocation == this)
			character.CurrentLocation = null;
	}

	public void SetBuildingType(BuildingType type)
	{
		BuildType = type;

		Sprite.Texture = ResourceLoader.Load<Texture>($@"Assets\Sprites\Buildings\{type.SpriteLocation}.png");
	}

	void OnAreaEntered(Area2D area)
	{
		if(area is Character)
		{
			EnterBuilding(area as Character);
		}
	}

	void OnAreaExited(Area2D area)
	{
		if (area is Character)
		{
			ExitBuilding(area as Character);
		}
	}
}
