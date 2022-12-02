using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Character/Explosion Events Channel")]
public class ExplosionEventsChannelSO : ScriptableObject
{
    public SpawnEventChannelSO SpawnEvent;
    public FloatEventChannelSO ResizeEvent;
    public DamageEventChannelSO HitEvent;
    public GameObjectEventChannelSO DespawnEvent;
}
