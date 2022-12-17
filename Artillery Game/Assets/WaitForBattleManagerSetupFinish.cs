using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForBattleManagerSetupFinish : MonoBehaviour
{
    [SerializeField] BattleManagerEventsChannelSO battleEvents;

    private void Awake() {
        battleEvents.SetupFinishEvent.OnEventRaised += EnableComponent;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnDisable() {
        battleEvents.SetupFinishEvent.OnEventRaised -= EnableComponent;
    }

    void EnableComponent()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
