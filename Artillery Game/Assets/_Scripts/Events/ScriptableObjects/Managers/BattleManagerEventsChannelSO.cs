using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Managers/BattleManager Events Channel")]
public class BattleManagerEventsChannelSO : ScriptableObject
{
    // public GameObjectEventChannelSO CharacterSpawnEvent;
    // public GameObjectEventChannelSO CharacterDespawnEvent;
    public VoidEventChannelSO SetupFinishEvent;
    public VoidEventChannelSO EndBattleEvent;
}
