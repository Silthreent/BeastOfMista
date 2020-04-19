using Godot;
using System;
using System.Collections.Generic;

public class WorldManager : Node2D
{
	public static WorldManager World { get; protected set; }
	public Navigation2D NavMesh { get; protected set; }

	Node2D BuildingNode;
	List<Building> Buildings;
	List<Character> Villagers;

	public WorldManager()
	{
		World = this;

		Buildings = new List<Building>();
		Villagers = new List<Character>();
	}

	public override void _Ready()
	{
		NavMesh = FindNode("NavMesh") as Navigation2D;

		BuildingNode = GetNode<Node2D>("Buildings");

		Villagers.Add(FindNode("Character") as Character);
	}

	public Building CreateBuilding()
	{
		var building = ResourceLoader.Load<PackedScene>(@"Scenes\World\Building.tscn").Instance() as Building;
		BuildingNode.AddChild(building);

		if (GetBuilding(x => x.BuildType is TownHall) == null)
		{
			building.SetBuildingType(new TownHall());
		}
		else if (GetBuildingType(x => x.BuildType is Farmland).Length < Villagers.Count / 3f)
		{
			building.SetBuildingType(new Farmland());
		}
		else
		{
			building.SetBuildingType(new BuildingType());
		}

		return building;
	}

	public void RegisterBuilding(Building building)
	{
		Buildings.Add(building);

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
