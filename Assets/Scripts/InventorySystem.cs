using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<string> inventory = new List<string>();

    public void AddItem(string item)
    {
        inventory.Add(item);
        Debug.Log(item + " added to inventory.");
    }
}
