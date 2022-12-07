using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_AngleDisplay : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] TurnsManagerEventsChannelSO turnsManagerEvents;

    Aiming currentChar;
    TextMeshProUGUI text;

    private void Awake() 
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable() 
    {
        turnsManagerEvents.StartTurn.OnEventRaised += GetCurrentChar;
    }

    private void OnDisable() 
    {
        turnsManagerEvents.StartTurn.OnEventRaised -= GetCurrentChar;
    }

    private void Update() 
    {
        if(currentChar != null)
        text.text = currentChar.angle.ToString() + " °";
    }

    void GetCurrentChar()
    {
        currentChar = turnsManagerEvents.currentChar.GetComponent<Aiming>();
    }
}
