using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinningTextScript : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet characterRuntimeSet;
    TextMeshProUGUI text;

    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        var team = characterRuntimeSet.Items[0].GetComponent<CharManager>().team;

        text.text = (team.name + " Wins!");
        text.color = team.color;
    }
}
