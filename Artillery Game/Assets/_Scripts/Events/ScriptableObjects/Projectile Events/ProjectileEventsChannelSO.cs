using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Character/Projectile Events Channel")]
public class ProjectileEventsChannelSO : ScriptableObject
{
    public SpawnEventChannelSO SpawnEvent;
    public Vector3EventChannelSO LaunchEvent;
    public Vector3EventChannelSO TrajectoryEvent;
    public DamageEventChannelSO HitEvent;
    public SpawnEventChannelSO ExplodeEvent;
    public GameObjectEventChannelSO DespawnEvent;
}
