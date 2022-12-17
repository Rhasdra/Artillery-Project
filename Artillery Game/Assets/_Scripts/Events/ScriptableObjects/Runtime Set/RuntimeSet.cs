using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSet<T> : ScriptableObject
{
    public List<T> Items = new List<T>();
    public VoidEventChannelSO OnItemAdd;
    public VoidEventChannelSO OnItemRemove;
    
    public void Add (T t)
    {
        if(!Items.Contains(t))
            {
                Items.Add(t);
                OnItemAdd.RaiseEvent();
            }
    }

    public void Remove (T t)
    {
        if (Items.Contains(t))
            {
                Items.Remove(t);
                OnItemRemove.RaiseEvent();
            }
    }

    public void Clear()
    {
        if(Items != null)
            {
                Items.Clear();
            }
    }
}
