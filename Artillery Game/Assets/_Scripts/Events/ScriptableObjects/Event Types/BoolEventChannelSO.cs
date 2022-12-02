using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Bool Event Channel")]
public class BoolEventChannelSO : ScriptableObject
{
    public UnityAction<bool> OnEventRaised = delegate { };

    public void RaiseEvent(bool boolean)
    {
        OnEventRaised.Invoke(boolean);
    }
}
