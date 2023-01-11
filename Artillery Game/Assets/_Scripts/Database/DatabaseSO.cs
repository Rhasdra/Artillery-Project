using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DatabaseSO<T> : ScriptableObject, ISerializationCallbackReceiver
{
    public T[] objects;
    public Dictionary<T, int> GetId = new Dictionary<T, int>();
    public Dictionary<int, T> GetObject = new Dictionary<int, T>();

    public void OnAfterDeserialize()
    {
        if(objects == null)
            return;

        GetId = new Dictionary<T, int>(); 
        GetObject = new Dictionary<int, T>();     
        for (int i = 0; i < objects.Length; i++)
        {
            GetId.Add(objects[i], i);
            GetObject.Add(i, objects[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
