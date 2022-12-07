using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : MonoBehaviour
{
    [Header("Listening to")]
    WeaponEventsChannelSO weaponEvents;

    public int delay;

    void RandomizeDelay()
    {
        delay = Random.Range(0,255);
    }
}
