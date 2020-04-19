using System;
using System.Collections.Generic;

public class StatManager
{
    Dictionary<Stat, float> StatList;

    public StatManager()
    {
        StatList = new Dictionary<Stat, float>();

        foreach(Stat x in Enum.GetValues(typeof(Stat)))
        {
            StatList.Add(x, 0);
            StatList[x] = 25;
        }
    }

    public void RestoreStat(Stat stat, float amount)
    {
        StatList[stat] += amount;

        if (StatList[stat] >= 100)
        {
            StatList[stat] = 100;
        }
    }

    public void ReduceStat(Stat stat, float amount)
    {
        StatList[stat] -= amount;

        if(StatList[stat] <= 0)
        {
            StatList[stat] = 0;
        }
    }

    public float GetStat(Stat stat)
    {
        return StatList[stat];
    }
}

public enum Stat
{
    Energy
}
