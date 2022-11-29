using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WinProfit
{
    public float totalValue;
    public float winValue;
    public float rawCurrency;


    public WinProfit(float run, float win, float currency)
    {
        totalValue = run;
        winValue = win;
        rawCurrency = currency;
    }
}
