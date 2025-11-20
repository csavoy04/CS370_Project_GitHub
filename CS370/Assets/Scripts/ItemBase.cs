using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]
public class Item
{

    public ItemType Type;
    public int ItemCount;
    public string Description;

    public enum ItemType
    {
        Empty,
        HealthPotion,
        ManaPotion
    }

    public Item(ItemType GivenType, int GivenCount, string description)
    {
        Type = GivenType;
        ItemCount = GivenCount;
        Description = description;
    }

    public void SetType(ItemType NewType)
    {
        Type = NewType;
    }

    public void SetDescription(string NewDescription)
    {
        Description = NewDescription;
    }

    public ItemType GetType()
    {
        return Type;
    }

    public int GetCount()
    {
        return ItemCount;
    }

    public string GetDescription()
    {
        return Description;
    }

    public void UseItem(int NumUsed)
    {
        ItemCount -= NumUsed;
    }

    public void AddItem(int NumAdded)
    {
        ItemCount += NumAdded;
    }
}