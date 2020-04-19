public class BuildingType
{
    public string SpriteLocation { get; protected set; } = "Building";

    public virtual void ProcessPatron(Character character, float delta) { }
}
