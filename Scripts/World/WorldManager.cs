using Godot;
using System;
using System.Collections.Generic;

public class WorldManager : Node2D
{
	[Export]
	public PackedScene CharacterScene;

	public static WorldManager World { get; protected set; }
	public Navigation2D NavMesh { get; protected set; }
	public Random GlobalRNG { get; protected set; }

	Node2D BuildingNode;
	Node2D CharacterNode;
	List<Building> Buildings;
	List<Character> Villagers;


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

	public Building CreateBuilding()
	{
		var building = ResourceLoader.Load<PackedScene>(@"Scenes\World\Building.tscn").Instance() as Building;
		Buildings.Add(building);
		BuildingNode.AddChild(building);

		if (GetBuilding(x => x.BuildType is TownHall) == null)
		{
			building.SetBuildingType(new TownHall());
			building.Storage.GainItem(Item.Wheat, 1000);
		}
		else if (GetBuildingType(x => x.BuildType is Farmland).Length < Villagers.Count / 2f)
		{
			building.SetBuildingType(new Farmland());
		}
		else
		{
			building.SetBuildingType(new House());
		}

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

	public Building[] GetBuildingType(Predicate<Building> check)
	{
		return Buildings.FindAll(check).ToArray();
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

	public override void _UnhandledKeyInput(InputEventKey input)
	{
		if(input.IsActionPressed("quit_game"))
		{
			GetTree().Quit();
		}
	}
}
