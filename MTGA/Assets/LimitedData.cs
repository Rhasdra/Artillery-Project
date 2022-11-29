using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LimitedData
{
    public LimitedEvent limitedEvent;
    public float profitChance;
    public float infiniteChance;

    public float expectedTotalValue;
    public float expectedWinProfit;
    public float expectedRawCurrency;
    public List<WinProfit> winProfits;
}
