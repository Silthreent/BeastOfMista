using Godot;
using System;
using System.Collections.Generic;

public class WorldManager : Node2D
{
	public const int REQUIRED_BUILDING_MATERIALS = 300;

	[Export]
	public PackedScene CharacterScene;

	public static WorldManager World { get; protected set; }
	public Navigation2D NavMesh { get; protected set; }
	public Random GlobalRNG { get; protected set; }

	Building TownHall;
	Node2D BuildingNode;
	Node2D CharacterNode;
	List<Building> Buildings;
	List<Character> Villagers;

	CursorMode Cursor;

	public WorldManager()
	{
		World = this;

		Buildings = new List<Building>();
		Villagers = new List<Character>();

		GlobalRNG = new Random();
	}

	public override void _Ready()
	{
		NavMesh = FindNode("NavMesh") as Navigation2D;

		BuildingNode = GetNode<Node2D>("Buildings");
		CharacterNode = GetNode<Node2D>("Characters");

		var builder = CreateVillager();
		builder.SetJob(new BuilderAI(builder));
		builder.Inventory.GainItem(Item.Wheat, 300);

		var farmer = CreateVillager();
		farmer.SetJob(new FarmerAI(farmer));
	}

	public Building GetNextBuildProject()
	{
		var inprogress = GetBuilding(x => !x.IsFunctional && x.AssignedBuilder == null);
		if (inprogress != null)
		{
			GetTownHall().Storage.LoseItem(Item.Wheat, REQUIRED_BUILDING_MATERIALS / inprogress.BuildType.MaxBuildProgress);
			return inprogress;
		}

		if (GetTownHall() == null)
		{
			var build = CreateBuilding();
			build.SetBuildingType(new TownHall());
			build.Storage.GainItem(Item.Wheat, 2500);
			TownHall = build;

			return build;
		}
		else if(GetTownHall().Storage[Item.Wheat] >= REQUIRED_BUILDING_MATERIALS)
		{
			GetTownHall().Storage.LoseItem(Item.Wheat, REQUIRED_BUILDING_MATERIALS);

			if (GetBuildingType(x => x.BuildType is Farmland).Length < Villagers.Count / 2f)
			{
				var build = CreateBuilding();
				build.SetBuildingType(new Farmland());

				return build;
			}
			else
			{
				var build = CreateBuilding();
				build.SetBuildingType(new House());

				return build;
			}
		}

		return null;
	}

	Building CreateBuilding()
	{
		var building = ResourceLoader.Load<PackedScene>(@"Scenes\World\Building.tscn").Instance() as Building;
		Buildings.Add(building);
		BuildingNode.AddChild(building);

		return building;
	}

	public void RegisterBuilding(Building building)
	{
		var polyMesh = NavMesh.FindNode("NavPoly") as NavigationPolygonInstance;

		var outPoly = new List<Vector2>();
		var finalTransform = polyMesh.GlobalTransform.Inverse() * building.PathCollider.GlobalTransform;
		foreach(var x in building.PathCollider.Polygon)
		{
			outPoly.Add(finalTransform.Xform(x));
		}

		polyMesh.Navpoly.AddOutline(outPoly.ToArray());
		polyMesh.Navpoly.MakePolygonsFromOutlines();
		
		polyMesh.Enabled = false;
		polyMesh.Enabled = true;
	}

	public Building GetBuilding(Predicate<Building> check)
	{
		return Buildings.Find(check);
	}

	public Building GetTownHall()
	{
		return TownHall;
	}

	public Building[] GetBuildingType(Predicate<Building> check)
	{
		return Buildings.FindAll(check).ToArray();
	}

	public int GetBuildingCount()
	{
		return Buildings.Count;
	}

	public Character CreateVillager()
	{
		var newVillager = CharacterScene.Instance() as Character;

		Villagers.Add(newVillager);
		CharacterNode.AddChild(newVillager);

		return newVillager;
	}

	public int GetVillagerCount()
	{
		return Villagers.Count;
	}

	public void BuildingClicked(Building building)
	{
		if(Cursor == CursorMode.Destroy)
		{
			if(!(building.BuildType is TownHall))
				building.TakeDamage(20);
		}
	}

	public override void _UnhandledKeyInput(InputEventKey input)
	{
		if(input.IsActionPressed("unlock_cursor"))
		{
			if(Input.GetMouseMode() == Input.MouseMode.Confined)
			{
				Input.SetMouseMode(Input.MouseMode.Visible);
			}
			else
			{
				Input.SetMouseMode(Input.MouseMode.Confined);
			}
		}
		else if(input.IsActionPressed("cursor_destroy"))
		{
			if (Cursor != CursorMode.Destroy)
				Cursor = CursorMode.Destroy;
			else
				Cursor = CursorMode.Nothing;
		}
	}
}

enum CursorMode
{
	Nothing,
	Destroy
}
