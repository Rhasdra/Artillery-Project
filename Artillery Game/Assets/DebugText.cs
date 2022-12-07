using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugText : Billboard
{
    TextMeshProUGUI text;
    public CharManager currentCharManager;

    private void Awake() 
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(CharManager charManager)
    {
        followPoint = charManager.transform;
        currentCharManager = charManager;
    }

    private void Update() 
    {
        text.text = 
        "Health: " + currentCharManager.health + "\n" +
        "Power: " + currentCharManager.power + "\n" +
        "Angle: " + currentCharManager.angle + "\n" +
        "Weapon: " + currentCharManager.weaponIndex + "\n" +
        "Delay: " + currentCharManager.delay + "\n"
        ;
    }
}
