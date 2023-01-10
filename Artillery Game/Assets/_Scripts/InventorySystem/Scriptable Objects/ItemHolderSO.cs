using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemHolder", menuName = "Inventory/Item Holder")]
public class ItemHolderSO : ScriptableObject
{
    public ItemSO Item;
}
