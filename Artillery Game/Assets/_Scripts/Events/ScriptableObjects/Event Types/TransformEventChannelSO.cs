using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Transform Event Channel")]
public class TransformEventChannelSO : ScriptableObject
{
    public UnityAction<Transform> OnEventRaised = delegate { };

    public void RaiseEvent(Transform t)
    {
        OnEventRaised.Invoke(t);
    }
}