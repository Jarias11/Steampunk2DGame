using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ItemIDValidator : EditorWindow {
    [MenuItem("Tools/Validate Item IDs")]
    public static void ValidateItemIDs() {
        string[] guids = AssetDatabase.FindAssets("t:Item", new[] { "Assets/GameData/Resources/Items" });
        Dictionary<string, Item> idMap = new Dictionary<string, Item>();
        HashSet<string> seenIDs = new HashSet<string>();

        int fixedCount = 0;
        int missingCount = 0;
        int duplicateCount = 0;

        foreach (string guid in guids) {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Item item = AssetDatabase.LoadAssetAtPath<Item>(path);

            if (item == null) continue;

            // Check for missing ID
            if (string.IsNullOrEmpty(item.itemID)) {
                item.itemID = System.Guid.NewGuid().ToString();
                EditorUtility.SetDirty(item);
                Debug.LogWarning($"🔧 Assigned new ID to: {item.name}");
                missingCount++;
                fixedCount++;
                continue;
            }

            // Check for duplicates
            if (seenIDs.Contains(item.itemID)) {
                Debug.LogError($"❌ Duplicate itemID detected: {item.itemID} on item {item.name}");
                duplicateCount++;
            }
            else {
                seenIDs.Add(item.itemID);
                idMap[item.itemID] = item;
            }
        }

        AssetDatabase.SaveAssets();

        Debug.Log($"✅ Item ID Validation Complete: {missingCount} missing, {duplicateCount} duplicates, {fixedCount} fixed.");
    }
}