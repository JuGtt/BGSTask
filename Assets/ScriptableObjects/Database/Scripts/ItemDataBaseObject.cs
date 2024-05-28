using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "New Database")]
public class ItemDataBaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    #region Public Fields
    public List<ItemSO> Items;
    public Dictionary<int, ItemSO> GetItem = new Dictionary<int, ItemSO>();
    [Tooltip("Used to populate automatically from any folder.")]
    public string FolderPath;
    #endregion

    #region Public Methods
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].ID = i;
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemSO>();
    }

    public ItemSO GetItemById(int id)
    {
        return GetItem.ContainsKey(id) ? GetItem[id] : null;
    }
    public void FetchFromPath()
    {
        if(Items != null) Items.Clear(); // Clear the existing items

        string[] guids = AssetDatabase.FindAssets("t:ItemSO", new[] { FolderPath });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ItemSO item = AssetDatabase.LoadAssetAtPath<ItemSO>(assetPath);
            if (item != null)
            {
                Items.Add(item);
            }
        }

        // Re-populate the dictionary
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].ID = i;
            GetItem[i] = Items[i];
        }
    }
    #endregion

}
