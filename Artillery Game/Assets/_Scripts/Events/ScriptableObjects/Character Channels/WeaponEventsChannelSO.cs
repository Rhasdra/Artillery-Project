using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Character/Weapon Events Channel")]
public class WeaponEventsChannelSO : ScriptableObject
{
    public VoidEventChannelSO ShootEvent;
    public IntEventChannelSO ShootDelayEvent;
    public VoidEventChannelSO WeaponChangeEvent;
    public IntEventChannelSO WeaponChangeIndexEvent;
}
