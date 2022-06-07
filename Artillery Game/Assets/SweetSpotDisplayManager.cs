using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SweetSpotDisplayManager : MonoBehaviour
{
    public Image fill;

    public void SSFill(float amount)
    {
        fill.fillAmount = amount;
    }
}
