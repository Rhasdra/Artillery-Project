using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Character/Movement Events Channel")]
public class MovementEventsChannelSO : ScriptableObject
{
    [Header("Horizontal Movement Events")]
    public TransformEventChannelSO MoveStartEvent;
    public Vector3EventChannelSO MoveStopEvent;
    [Header("Jump Events")]
    public TransformEventChannelSO LongJumpEvent;
    public TransformEventChannelSO BackFlipJumpEvent;
    public Vector3EventChannelSO LandingEvent;
    [Header("Delay Events")]
    public VoidEventChannelSO ThresholdCrossedEvent;
}
