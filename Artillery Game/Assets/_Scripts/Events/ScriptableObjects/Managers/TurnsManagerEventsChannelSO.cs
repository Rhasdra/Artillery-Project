using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Managers/TurnsManager Events Channel")]
public class TurnsManagerEventsChannelSO : ScriptableObject
{
    public VoidEventChannelSO SetupFinishEvent;
    public VoidEventChannelSO StartTurn;
    public VoidEventChannelSO EndTurn;
    public VoidEventChannelSO NewCycle;

    public CharManager charTakingTurn;
    public List<CharManager> charList;
}
