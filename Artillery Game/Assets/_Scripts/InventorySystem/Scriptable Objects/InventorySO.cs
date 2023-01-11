using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory/Inventory")]
public class InventorySO : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemDatabaseSO database;
    public int maxSlots = 3;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public void AddItem(ItemSO _item, int _amount)
    {
        foreach (var slot in slots)
        {
            if(slot.item == _item)
                slot.AddAmount(_amount);
                return;                
        }

        slots.Add(new InventorySlot(database.GetId[_item], _item, _amount));
    }

    public void ClearInventory()
    {
        slots.Clear();
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].item = database.GetObject[slots[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemSO item;
    public int amount;
    
    public InventorySlot(int _id, ItemSO _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
