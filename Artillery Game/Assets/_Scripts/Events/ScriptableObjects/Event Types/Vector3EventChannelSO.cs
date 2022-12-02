using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Vector3 Event Channel")]
public class Vector3EventChannelSO : ScriptableObject
{
    public UnityAction<Vector3> OnEventRaised = delegate { };

    public void RaiseEvent(Vector3 position)
    {
        OnEventRaised.Invoke(position);
    }
}
