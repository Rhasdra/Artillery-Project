using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Clickable : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent LeftClick;
    public UnityEvent RightClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Use this to tell when the user right-clicks on the Button
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick.Invoke();
        }

        //Use this to tell when the user left-clicks on the Button
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick.Invoke();
        }
    }
}
