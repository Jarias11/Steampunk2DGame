using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    private Dictionary<string, Item> itemDict = new Dictionary<string, Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadItems()
    {
        Item[] allItems = Resources.LoadAll<Item>("Items");
        foreach (Item item in allItems)
        {
            if (!string.IsNullOrEmpty(item.itemID) && !itemDict.ContainsKey(item.itemID))
            {
                itemDict.Add(item.itemID, item);
            }
        }
    }

    public static Item GetItemByID(string id)
    {
        if (Instance == null) return null;
        return Instance.itemDict.TryGetValue(id, out Item item) ? item : null;
    }
}