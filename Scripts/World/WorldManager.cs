using Godot;
using System;
using System.Collections.Generic;

public class WorldManager : Node2D
{
	public static WorldManager World { get; protected set; }
	public Navigation2D NavMesh { get; protected set; }

	Node2D BuildingNode;
	List<Building> Buildings;

	public WorldManager()
	{
		World = this;

		Buildings = new List<Building>();
	}

	public override void _Ready()
	{
		NavMesh = FindNode("NavMesh") as Navigation2D;

		BuildingNode = GetNode<Node2D>("Buildings");
	}

	public Building CreateBuilding()
	{
		var building = ResourceLoader.Load<PackedScene>(@"Scenes\World\Building.tscn").Instance() as Building;
		BuildingNode.AddChild(building);

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
		foreach(var x in Buildings)
		{
			if(check(x))
			{
				return x;
			}
		}

		return null;
	}

	public override void _UnhandledKeyInput(InputEventKey input)
	{
		if(input.IsActionPressed("quit_game"))
		{
			GetTree().Quit();
		}
	}
}
