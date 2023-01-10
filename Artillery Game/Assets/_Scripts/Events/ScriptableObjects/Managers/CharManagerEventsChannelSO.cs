using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Managers/CharManager Events Channel")]
public class CharManagerEventsChannelSO : ScriptableObject
{
    public VoidEventChannelSO EndTurn;
    public GameObjectEventChannelSO CharacterDeath;
}