public class House : BuildingType
{
    public House()
    {
        SpriteLocation = "House";

        MaxBuildProgress = 25f;
    }

    public override void FinishedBuilding(Building building)
    {
        var newVillager = WorldManager.World.CreateVillager();

        if(WorldManager.World.GetBuilding(x =>
        x.BuildType is Farmland 
        && (x.BuildType as Farmland).AssignedWorker == null) != null)
        {
            newVillager.SetJob(new FarmerAI(newVillager));
        }
        else
        {
            newVillager.SetJob(new BuilderAI(newVillager));
        }

        newVillager.GlobalPosition = building.GlobalPosition;
    }
}
