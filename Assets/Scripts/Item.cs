using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Item.cs
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string description;
    public string itemType; // e.g., "Weapon", "Gear", "Consumable"

    public Item(string name, string desc, string type)
    {
        itemName = name;
        description = desc;
        itemType = type;
    }

    public override string ToString()
    {
        return $"{itemName} ({itemType})";
    }
}