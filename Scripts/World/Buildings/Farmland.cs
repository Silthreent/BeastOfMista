public class Farmland : BuildingType
{
    public Character AssignedWorker { get; protected set; }

    public Farmland()
    {
        SpriteLocation = "Farmland";

        MaxBuildProgress = 2f;
    }

    public void AssignWorker(Character character)
    {
        AssignedWorker = character;
    }
}
