using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Managers/WindManager Events Channel")]
public class WindManagerEventsChannelSO : ScriptableObject
{
    public VoidEventChannelSO WindChange;
    public FloatEventChannelSO WindDirectionChange;
    public FloatEventChannelSO WindStrengthChange;
}