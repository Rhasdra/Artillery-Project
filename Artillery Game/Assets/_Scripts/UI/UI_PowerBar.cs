using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PowerBar : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] TurnsManagerEventsChannelSO turnsManagerEvents;
    
    [SerializeField] Aiming currentChar;
    Slider slider = null;
    TextMeshProUGUI text = null;

    bool isBeingDragged = false;

    private void Awake() 
    {
        slider = GetComponentInChildren<Slider>();
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
        if(currentChar != null && isBeingDragged == false)
        {
            slider.value = currentChar.power;
            text.text = slider.value.ToString();
        }
        else if(currentChar == null) // In case reference gets lost
        {
            GetCurrentChar();
        }
    }

    void GetCurrentChar()
    {
        currentChar = turnsManagerEvents.charTakingTurn.GetComponent<Aiming>();
    }

    public void OnBeginDrag()
    {
        isBeingDragged = true;
    }

    public void OnReleaseDrag()
    {
        currentChar.power = slider.value;
        isBeingDragged = false;
    }
}
