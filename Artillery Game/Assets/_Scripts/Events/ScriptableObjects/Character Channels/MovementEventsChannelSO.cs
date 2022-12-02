using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Character/Movement Events Channel")]
public class MovementEventsChannelSO : ScriptableObject
{
    public TransformEventChannelSO MoveStartEvent;
    public Vector3EventChannelSO MoveStopEvent;
    public TransformEventChannelSO LongJumpEvent;
    public TransformEventChannelSO BackFlipJumpEvent;
    public Vector3EventChannelSO LandingEvent;
}
