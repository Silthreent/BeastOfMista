public class Farmland : BuildingType
{
    public Character AssignedWorker { get; protected set; }
    public float WheatStockpile { get; protected set; }

    public Farmland()
    {
        SpriteLocation = "Farmland";

        MaxBuildProgress = 10f;
    }

    public void AssignWorker(Character character)
    {
        AssignedWorker = character;
    }

    public void IncreaseStockpile(float amount)
    {
        WheatStockpile += amount;
    }
}
