using Godot;
using System.Collections.Generic;

public class WorldManager : Node2D
{
	public static WorldManager World { get; protected set; }
	public Navigation2D NavMesh { get; protected set; }

	List<Building> Buildings;

	public WorldManager()
	{
		World = this;

		Buildings = new List<Building>();
	}

	public override void _Ready()
	{
		NavMesh = FindNode("NavMesh") as Navigation2D;
	}

	public void RegisterBuilding(Building building)
	{
		Buildings.Add(building);

		var polyMesh = NavMesh.FindNode("NavPoly") as NavigationPolygonInstance;

		var outPoly = new List<Vector2>();
		var finalTransform = polyMesh.GlobalTransform.Inverse() * building.Collider.GlobalTransform;
		foreach(var x in building.Collider.Polygon)
		{
			outPoly.Add(finalTransform.Xform(x));
		}

		polyMesh.Navpoly.AddOutline(outPoly.ToArray());
		polyMesh.Navpoly.MakePolygonsFromOutlines();
		
		polyMesh.Enabled = false;
		polyMesh.Enabled = true;
	}

	public Building GetBuilding()
	{
		return Buildings[0];
	}
}
