public class TownHall : BuildingType
{
    public TownHall()
    {
        SpriteLocation = "TownHall";

        MaxBuildProgress = 2f;
    }

    public override void ProcessPatron(Character character, float delta)
    {
        if(character.AI.CurrentState is RelaxState)
            character.Stats.RestoreStat(Stat.Energy, 10 * delta);
    }
}
