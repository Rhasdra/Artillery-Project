using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Spawn Event Channel")]
public class SpawnEventChannelSO : ScriptableObject
{
    public UnityAction<GameObject, Vector3> OnEventRaised;

    public void RaiseEvent(GameObject prefab, Vector3 position)
    {
        OnEventRaised.Invoke(prefab, position);
    }
}