public class Farmland : BuildingType
{
    public Character AssignedWorker { get; protected set; }

    public Farmland()
    {
        SpriteLocation = "Farmland";

        MaxBuildProgress = 15f;
    }

    public void AssignWorker(Character character)
    {
        AssignedWorker = character;
    }

    public override void VillagerDied(Character villager)
    {
        AssignedWorker = null;
    }
}
