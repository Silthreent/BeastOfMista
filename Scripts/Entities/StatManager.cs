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
            StatList.Add(x, 100);
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

    public float this[Stat stat]
    {
        get
        {
            return StatList[stat];
        }
    }
}

public enum Stat
{
    Energy
}
