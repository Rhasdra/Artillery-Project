using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject, ISerializationCallbackReceiver
{
    public string ItemName;
    public string ItemDescription;

    public List<int> componentID;
    public List<ProjectileComponent> components;

    public ProjectileComponentsDatabaseSO database;

    private void Awake() 
    {
        database = Resources.Load<ProjectileComponentsDatabaseSO>("Database/ProjectileComponentsDatabase");
    }

    public void OnAfterDeserialize()
    {
        components.Clear();

        foreach (var value in componentID)
        {
            components.Add(database.GetObject[value]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}