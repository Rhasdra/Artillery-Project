using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Character/Weapon Events Channel")]
public class WeaponEventsChannelSO : ScriptableObject
{
    public SpawnEventChannelSO ShootEvent;
    public GameObjectEventChannelSO WeaponChangeEvent;
}
