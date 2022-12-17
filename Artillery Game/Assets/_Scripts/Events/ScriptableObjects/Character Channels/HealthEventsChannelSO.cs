using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Managers/Health Events Channel")]
public class HealthEventsChannelSO : ScriptableObject
{
    public GameObjectEventChannelSO CharacterDeath;
}