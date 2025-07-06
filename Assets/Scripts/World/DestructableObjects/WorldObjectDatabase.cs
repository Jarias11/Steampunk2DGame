using System.Collections.Generic;
using UnityEngine;

public class WorldObjectDatabase : MonoBehaviour
{
    public static WorldObjectDatabase Instance;

    [System.Serializable]
    public class PrefabEntry
    {
        public string prefabID;
        public GameObject prefab;
    }

    [Header("Registered World Object Prefabs")]
    public List<PrefabEntry> prefabEntries;

    private Dictionary<string, GameObject> prefabLookup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void BuildDictionary()
    {
        prefabLookup = new Dictionary<string, GameObject>();
        foreach (var entry in prefabEntries)
        {
            if (!prefabLookup.ContainsKey(entry.prefabID))
            {
                prefabLookup.Add(entry.prefabID, entry.prefab);
            }
            else
            {
                Debug.LogWarning($"Duplicate prefabID detected: {entry.prefabID}");
            }
        }
    }

    public GameObject GetPrefabByID(string id)
    {
        if (prefabLookup.TryGetValue(id, out GameObject prefab))
        {
            return prefab;
        }

        Debug.LogWarning($"Prefab ID not found: {id}");
        return null;
    }
}