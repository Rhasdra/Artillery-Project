using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySO inventory;

    private void Start() 
    {
        ClearInventory(inventory);
        ConstructInventory(inventory);
    }

    void ConstructInventory(InventorySO _inventory)
    {
        while(_inventory.itemHolders.Count < _inventory.Slots)
        {
            ItemHolderSO newHolder = ScriptableObject.CreateInstance<ItemHolderSO>();
            _inventory.itemHolders.Add(newHolder);
        }
    }

    void ClearInventory(InventorySO _inventory)
    {
        _inventory.ClearInventory();
    }
}
