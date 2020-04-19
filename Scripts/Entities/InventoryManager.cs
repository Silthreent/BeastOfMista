using System;
using System.Collections.Generic;

public class InventoryManager
{
    Dictionary<Item, float> ItemList;

    public InventoryManager()
    {
        ItemList = new Dictionary<Item, float>();

        foreach (Item x in Enum.GetValues(typeof(Item)))
        {
            ItemList.Add(x, 0);
        }
    }

    public void GainItem(Item item, float amount)
    {
        ItemList[item] += amount;
    }

    public void LoseItem(Item item, float amount)
    {
        ItemList[item] -= amount;

        if (ItemList[item] <= 0)
        {
            ItemList[item] = 0;
        }
    }

    public float this[Item item]
    {
        get
        {
            return ItemList[item];
        }
    }
}

public enum Item
{
    Wheat,
    Wood,
    Stone
}
