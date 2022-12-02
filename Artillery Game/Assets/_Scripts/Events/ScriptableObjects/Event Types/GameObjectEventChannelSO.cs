using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/GameObject Event Channel")]
public class GameObjectEventChannelSO : ScriptableObject
{
    public UnityAction<GameObject> OnEventRaised = delegate { };

    public void RaiseEvent(GameObject go)
    {
        OnEventRaised.Invoke(go);
    }
}