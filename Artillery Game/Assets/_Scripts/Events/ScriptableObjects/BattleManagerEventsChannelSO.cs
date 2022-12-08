using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Managers/BattleManager Events Channel")]
public class BattleManagerEventsChannelSO : ScriptableObject
{
    public GameObjectEventChannelSO CharacterSpawnEvent;
    public VoidEventChannelSO SetupFinishEvent;

    public List<GameObject> Characters;
    public List<GameObject> Projectiles;
    public VoidEventChannelSO EmptyProjectileList;
}
