using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory/Inventory")]
public class InventorySO : ScriptableObject
{
    public int Slots = 3;
    public List<ItemHolderSO> itemHolders;

    public void ClearInventory()
    {
        itemHolders.Clear();
    }
}
