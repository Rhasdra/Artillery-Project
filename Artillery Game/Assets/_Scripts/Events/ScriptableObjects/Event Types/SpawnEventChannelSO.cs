using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Spawn Event Channel")]
public class SpawnEventChannelSO : ScriptableObject
{
    public UnityAction<GameObject, Vector3, Quaternion> OnEventRaised = delegate { };

    public void RaiseEvent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        OnEventRaised.Invoke(prefab, position, rotation);
    }
}