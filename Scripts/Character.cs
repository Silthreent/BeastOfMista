using Godot;

namespace LudumDare46
{
	public class Character : Node2D
	{
		Line2D NavLine;

		public override void _Ready()
		{
			NavLine = FindNode("NavLine") as Line2D;
		}

		public override void _UnhandledInput(InputEvent input)
		{
			var mouse = input as InputEventMouseButton;
			if (mouse != null)
			{
				if(mouse.ButtonIndex == (int)ButtonList.Left && mouse.Pressed)
				{
					NavLine.GlobalPosition = new Vector2(0, 0);
					NavLine.Points = WorldManager.World.NavMesh.GetSimplePath(GlobalPosition, WorldManager.World.GetBuilding().Entrance.GlobalPosition);
				}
			}
		}
	}
}

