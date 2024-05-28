using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDataBaseObject))]
public class ItemDataBaseObjectEditor : Editor
{
    #region Override GUI Methods
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemDataBaseObject database = target as ItemDataBaseObject;

        EditorGUILayout.Space();

        if (GUILayout.Button("Fetch from folder"))
        {
            database.FetchFromPath();
            EditorUtility.SetDirty(database);
        }
    }
    #endregion
}
