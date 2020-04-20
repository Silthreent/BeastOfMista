using Godot;
using System.Collections.Generic;

public class Building : Node2D
{
	public Polygon2D PathCollider { get; protected set; }
	public Area2D InteractArea { get; protected set; }
	public BuildingType BuildType { get; protected set; }
	public float BuildProgress { get; protected set; }
	public bool IsCompleted { get; protected set; } = false;
	public bool IsFunctional {
		get
		{
			if(BuildProgress < BuildType.MaxBuildProgress)
			{
				return false;
			}
			else
			{
				return true;
			}
		} }
	public InventoryManager Storage { get; protected set; }
	public Character AssignedBuilder { get; protected set; }

	Sprite Sprite;
	List<Character> InsideBuilding;

	public override void _Ready()
	{
		InsideBuilding = new List<Character>();
		Storage = new InventoryManager();

		PathCollider = FindNode("PathCollider") as Polygon2D;

		InteractArea = FindNode("InteractArea") as Area2D;
		InteractArea.Connect("area_entered", this, "OnAreaEntered");
		InteractArea.Connect("area_exited", this, "OnAreaExited");
		InteractArea.Connect("input_event", this, "OnInputRecieved");

		Sprite = FindNode("Sprite") as Sprite;
	}

	public override void _Process(float delta)
	{
		if(IsCompleted)
		{
			foreach (var x in InsideBuilding)
			{
				BuildType.ProcessPatron(x, delta);
			}
		}
	}

	public void ProgressProgress(float amount)
	{
		BuildProgress += amount;

		if (BuildProgress >= BuildType.MaxBuildProgress)
		{
			CompleteBuilding();
		}
	}

	public void TakeDamage(float amount)
	{
		BuildProgress -= amount;
		if(BuildProgress < 0)
		{
			BuildProgress = 0;
		}
	}

	public void CompleteBuilding()
	{
		if(!IsCompleted)
			BuildType.FinishedBuilding(this);

		Sprite.Visible = true;
		IsCompleted = true;
		AssignedBuilder = null;
	}

	void EnterBuilding(Character character)
	{
		InsideBuilding.Add(character);
		character.Connect("VillagerDied", this, "OnVillagerDied");

		character.CurrentLocation = this;
	}

	void ExitBuilding(Character character)
	{
		InsideBuilding.Remove(character);
		character.Disconnect("VillagerDied", this, "OnVillagerDied");

		if (character.CurrentLocation == this)
			character.CurrentLocation = null;
	}

	public void SetBuildingType(BuildingType type)
	{
		BuildType = type;

		Sprite.Texture = ResourceLoader.Load<Texture>($@"Assets/Sprites/Buildings/{type.SpriteLocation}.png");
	}

	public void AssignBuilder(Character character)
	{
		AssignedBuilder = character;
		character.Connect("VillagerDied", this, "OnVillagerDied");
	}

	void OnVillagerDied(Character character)
	{
		ExitBuilding(character);

		if(AssignedBuilder == character)
		{
			AssignedBuilder = null;
		}

		BuildType.VillagerDied(character);
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

	void OnInputRecieved(Viewport viewport, InputEvent input, int shapexid)
	{
		if(input.IsActionPressed("interact"))
		{
			WorldManager.World.BuildingClicked(this);
		}
	}
}
