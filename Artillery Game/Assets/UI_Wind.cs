using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Wind : MonoBehaviour
{
    [Header("Listening to:")]
    [SerializeField] WindManagerEventsChannelSO windEvents;

    Image direction = null;
    TextMeshProUGUI strength = null;

    void Awake() 
    {
        direction = GetComponentInChildren<Image>();
        strength = GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnEnable() 
    {
        windEvents.WindDirectionChange.OnEventRaised += ChangeDirection;
        windEvents.WindStrengthChange.OnEventRaised += ChangeStrength;
    }

    void OnDisable() 
    {
        windEvents.WindDirectionChange.OnEventRaised -= ChangeDirection;
        windEvents.WindStrengthChange.OnEventRaised -= ChangeStrength;
    }

    void ChangeDirection(float value)
    {
        Vector3 newDirection = new Vector3(direction.transform.rotation.eulerAngles.x, direction.transform.rotation.eulerAngles.y, value);
        direction.rectTransform.rotation = Quaternion.Euler(newDirection);
    }

    void ChangeStrength(float value)
    {
        strength.text = null;

        int str = 0;
        for (int i = 0; i <= 4; i++)
        {
            if(str <= value && value > 0)
            {
                strength.text += "O";
                str ++;
            }
            else
            {
                strength.text += "•";
                str++;
            }

            if(i != 4)
            {
                strength.text += " ";
            }
        }
    }
}
