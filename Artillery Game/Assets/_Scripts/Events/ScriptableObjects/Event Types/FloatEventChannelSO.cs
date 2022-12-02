using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Float Event Channel")]
public class FloatEventChannelSO : ScriptableObject
{
    public UnityAction<float> OnEventRaised = delegate { };

    public void RaiseEvent(float value)
    {
        OnEventRaised.Invoke(value);
    }
}