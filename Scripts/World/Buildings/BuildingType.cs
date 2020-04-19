public class BuildingType
{
    public string SpriteLocation { get; protected set; } = "Building";
    public float MaxBuildProgress { get; protected set; } = 2f;

    public virtual void ProcessPatron(Character character, float delta) { }
}
