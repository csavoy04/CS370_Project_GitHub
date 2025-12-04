using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventorySystem : MonoBehaviour
{

    public List<Item> Inventory = null;
    public static InventorySystem Instance;

    //Prevents duplicates and keeps between scenes
    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // kill duplicate
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Inventory == null || Inventory.Count == 0)
        {

            //Create Stored Party
            Inventory = new List<Item>();

            for (int i = 0; i < 16; i++)
            {
                Inventory.Add(new Item(Item.ItemType.Empty, 0, ""));
            }

            Inventory.RemoveAt(0);
            Inventory.Insert(0, new Item(Item.ItemType.HealthPotion, 1, "Heals 1 Party Member"));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddItem(Item.ItemType type, int count, string description)
    {
        //Check for existing item
        foreach (Item item in Inventory)
        {
            if (item.GetType() == type)
            {
                item.AddItem(count);
                return;
            }
        }
        //Add new item to first empty slot
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].GetType() == Item.ItemType.Empty)
            {
                Inventory[i] = new Item(type, count, description);
                return;
            }
        }
    }

    public void UseItem(Item.ItemType type, int count, Unit Target)
    {
        //Find item in inventory
        foreach (Item item in Inventory)
        {
            //Check if item matches type
            if (item.GetType() == type)
            {
                //Use item if enough in stock
                if (item.GetCount() >= count)
                {
                    item.UseItem(count);
                    if (item.GetCount() <= 0)
                    {
                        item.SetType(Item.ItemType.Empty);
                        item.SetDescription("");
                    }

                    //Apply item effect
                    if(item.GetType() == Item.ItemType.HealthPotion)
                    {
                        Target.Heal(20 * count); //Heal 20 HP per potion used
                    }
                    else if(item.GetType() == Item.ItemType.ManaPotion)
                    {
                        Target.RestoreMana(20 * count); //Restore 20 MP per potion used
                    }

                    Debug.Log("Used " + count + " of " + type.ToString());
                }
                else
                {
                    //Not enough items
                    Debug.Log("Not enough items to use.");
                }
            }
        }
    }

    

}