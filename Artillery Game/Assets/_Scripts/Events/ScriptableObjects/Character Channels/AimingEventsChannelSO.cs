using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Character/Aiming Events Channel")]
public class AimingEventsChannelSO : ScriptableObject
{
    public IntEventChannelSO AngleChangeEvent;
    public IntEventChannelSO PowerChangeEvent;
    public BoolEventChannelSO SweetSpotEvent;
}
