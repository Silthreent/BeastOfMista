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
	public Building BeastCave { get; protected set; }

	Camera2D Camera;
	Building TownHall;
	Node2D BuildingNode;
	Node2D CharacterNode;
	List<Building> Buildings;
	List<Character> Villagers;
	Character Beast;

	Label BigText;
	ProgressBar HungerBar;
	TextureRect Selected;
	TextureRect[] UIIcons;
	Label HelpText;

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

		Camera = FindNode("Camera") as Camera2D;
		BuildingNode = GetNode<Node2D>("Buildings");
		CharacterNode = GetNode<Node2D>("Characters");
		BigText = FindNode("BigText") as Label;
		HungerBar = FindNode("HungerBar") as ProgressBar;
		Selected = FindNode("Selected") as TextureRect;
		HelpText = FindNode("HelpText") as Label;

		UIIcons = new TextureRect[4];
		UIIcons[0] = FindNode("Destroy") as TextureRect;
		UIIcons[1] = FindNode("Bless") as TextureRect;
		UIIcons[2] = FindNode("ReleaseBeast") as TextureRect;
		UIIcons[3] = FindNode("ReturnBeast") as TextureRect;

		var builder = CreateVillager();
		builder.SetJob(new BuilderAI(builder));
		builder.Inventory.GainItem(Item.Wheat, 300);

		var farmer = CreateVillager();
		farmer.SetJob(new FarmerAI(farmer));

		BeastCave = ResourceLoader.Load<PackedScene>(@"Scenes/World/Building.tscn").Instance() as Building;
		BuildingNode.AddChild(BeastCave);
		BeastCave.SetBuildingType(new BeastCave());
		BeastCave.ProgressProgress(10000);
		BeastCave.GlobalPosition = new Vector2(GlobalRNG.Next(700, 750), GlobalRNG.Next(700, 750));
		RegisterBuilding(BeastCave);

		Beast = CharacterScene.Instance() as Character;
		Beast.SetJob(new BeastAI(Beast));
		AddChild(Beast);
		Beast.GlobalPosition = BeastCave.GlobalPosition;
		Beast.SetSprite("Beast");
		Beast.MovementSpeed = 175f;
	}

	public override void _Process(float delta)
	{
		HungerBar.Value = (Beast.AI as BeastAI).Hunger;
	}

	public Building GetNextBuildProject()
	{
		if (GetTownHall() == null)
		{
			var build = CreateBuilding();
			build.SetBuildingType(new TownHall());
			build.Storage.GainItem(Item.Wheat, 2500);
			TownHall = build;

			return build;
		}

		var inprogress = GetBuilding(x => !x.IsFunctional && x.AssignedBuilder == null);
		if (inprogress != null)
		{
			GetTownHall().Storage.LoseItem(Item.Wheat, REQUIRED_BUILDING_MATERIALS / inprogress.BuildType.MaxBuildProgress);
			return inprogress;
		}
		
		if(GetTownHall().Storage[Item.Wheat] >= REQUIRED_BUILDING_MATERIALS)
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
		var building = ResourceLoader.Load<PackedScene>(@"Scenes/World/Building.tscn").Instance() as Building;
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
		newVillager.Connect("VillagerDied", this, "OnVillagerDied");
		Villagers.Add(newVillager);
		CharacterNode.AddChild(newVillager);

		return newVillager;
	}

	public void OnVillagerDied(Character character)
	{
		Villagers.Remove(character);

		if(GetVillagerCount() <= 0)
		{
			BigText.Text = "GAME OVER\n" +
				"Without any villagers to feast on, " +
	"your beast with refined tastes is bound to perish.";
			BigText.Visible = true;
		}
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

	public void TriggerEndCheck()
	{
		if (GlobalRNG.Next(0, GetVillagerCount()) > 10)
		{
			BigText.Text = "GAME OVER\n" +
			"The people have had enough." +
			"They head for your exhausted beast's cave, with no good intentions.";
			BigText.Visible = true; 

			foreach (var x in Villagers)
			{
				x.AI.SetState(new MovingState(BeastCave));
			}
		}
	}

	public override void _UnhandledKeyInput(InputEventKey input)
	{
		if (input.IsActionPressed("unlock_cursor"))
		{
			if (Input.GetMouseMode() == Input.MouseMode.Confined)
			{
				Input.SetMouseMode(Input.MouseMode.Visible);
			}
			else
			{
				Input.SetMouseMode(Input.MouseMode.Confined);
			}
		}
		else if (input.IsActionPressed("cursor_destroy"))
		{
			if (Cursor != CursorMode.Destroy)
			{
				Cursor = CursorMode.Destroy;
				Selected.RectGlobalPosition = UIIcons[0].RectGlobalPosition;
				Selected.Visible = true;
			}
			else
			{
				Cursor = CursorMode.Nothing;
				Selected.Visible = false;
			}
		}
		else if (input.IsActionPressed("cursor_bless"))
		{
			if (Cursor != CursorMode.Bless)
			{
				Cursor = CursorMode.Bless;
				Selected.RectGlobalPosition = UIIcons[1].RectGlobalPosition;
				Selected.Visible = true;
			}
			else
			{
				Cursor = CursorMode.Nothing;
				Selected.Visible = false;
			}
		}
		else if (input.IsActionPressed("switch_view"))
		{
			var beastDistance = Camera.GlobalPosition.DistanceTo(Beast.GlobalPosition);
			var townhallDistance = Camera.GlobalPosition.DistanceTo(GetTownHall() == null ? new Vector2(0, 0) : GetTownHall().GlobalPosition);

			if(beastDistance > townhallDistance)
			{
				Camera.GlobalPosition = Beast.GlobalPosition;
			}
			else
			{
				Camera.GlobalPosition = GetTownHall() == null ? new Vector2(0, 0) : GetTownHall().GlobalPosition;
			}
		}
		else if(input.IsActionPressed("release_beast"))
		{
			GD.Print("Required hunger: " + (100 - (((Beast.AI as BeastAI).Hunger - 100) / 2)));
			Beast.AI.SetState(new HuntState());
		}
		else if (input.IsActionPressed("return_beast"))
		{
			if(Beast.AI.CurrentState is HuntState)
			{
				if((Beast.AI as BeastAI).Hunger < (Beast.AI.CurrentState as HuntState).RequiredHunger)
				{
					Beast.AI.SetState(new MovingState(BeastCave));
				}
				else
				{
					GD.Print("TOO HUNGRY");
				}
			}

		}
	}
}

enum CursorMode
{
	Nothing,
	Destroy,
	Bless
}
