using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item), true)]
public class ItemEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        Item item = (Item)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Info", EditorStyles.boldLabel);
        EditorGUILayout.SelectableLabel(item.itemID ?? "(No ID assigned)", EditorStyles.textField);

        if (string.IsNullOrEmpty(item.itemID)) {
            EditorGUILayout.HelpBox("This item has no ID and may not be savable.", MessageType.Warning);
        }
    }
}