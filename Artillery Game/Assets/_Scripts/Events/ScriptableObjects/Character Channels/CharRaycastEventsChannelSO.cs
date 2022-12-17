using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Character/Raycast Events Channel")]
public class CharRaycastEventsChannelSO : ScriptableObject
{
    public BoolEventChannelSO IsGroundedEvent;
}
