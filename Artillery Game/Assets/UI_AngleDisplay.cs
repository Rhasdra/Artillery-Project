using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_AngleDisplay : MonoBehaviour
{
    Aiming currentChar;
    TextMeshProUGUI text;

    private void Awake() 
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable() 
    {
        TurnsManager.StartTurn.AddListener(GetCurrentChar);
    }

    private void Update() 
    {
        if(currentChar != null)
        text.text = currentChar.angle.ToString() + " °";
    }

    void GetCurrentChar()
    {
        currentChar = TurnsManager.currentChar.GetComponent<Aiming>();
    }
}
