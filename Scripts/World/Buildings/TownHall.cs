public class TownHall : BuildingType
{
    public TownHall()
    {
        SpriteLocation = "TownHall";
    }

    public override void ProcessPatron(Character character, float delta)
    {
        character.Stats.RestoreStat(Stat.Energy, 10 * delta);
    }
}
