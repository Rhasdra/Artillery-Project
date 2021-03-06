using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PowerBar : MonoBehaviour
{
    Aiming currentChar;
    Slider slider;
    TextMeshProUGUI text;

    bool isBeingDragged = false;

    private void Awake() 
    {
        slider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable() 
    {
        TurnsManager.StartTurn.AddListener(GetCurrentChar);
    }

    private void Update() 
    {
        if(currentChar != null && isBeingDragged == false)
        slider.value = currentChar.power;
        text.text = slider.value.ToString();
    }

    void GetCurrentChar()
    {
        currentChar = TurnsManager.currentChar.GetComponent<Aiming>();
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
