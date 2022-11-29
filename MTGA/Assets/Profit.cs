using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Profit
{
    public static int packCost = 1000;
    public static float gemCost = 20f/3f;

    public static void CalcProfits(LimitedEvent limited, List<WinProfit> winProfits)
    {
        for (int i = 0; i < limited.wins.Length; i++)
        {
            float entry()
            {
            if (limited.gold == 0)
                return limited.gems * gemCost;
            else
                return limited.gold;
            }

            float packGold()
            {
                return limited.wins[i].packs * packCost;
            }

            float currencyGold()
            {
                return limited.wins[i].gold + (limited.wins[i].gems * gemCost);
            }

            float totalValue = packGold() + currencyGold() + (limited.packsOpen * packCost);
            float winValue = totalValue - entry();
            float winRawCurrency = currencyGold() - entry();

            WinProfit thisWin = new WinProfit(totalValue, winValue, winRawCurrency);

            winProfits.Add(thisWin);
        }
    }

    public static void CalcExpectedProfits(LimitedData data)
    {
        // Expected Profit
        int wins = Mathf.FloorToInt(data.limitedEvent.winrateTable.expectedWins);
        float excess = data.limitedEvent.winrateTable.expectedWins - wins;

        Debug.Log(data.limitedEvent.winrateTable.expectedWins);

        float expectedTotalValue;
        float expectedWinProfit;
        float expectedRawProfit;

        if (wins == data.limitedEvent.wins.Length-1)
        {
            expectedTotalValue = data.winProfits[wins+1].totalValue;
            expectedWinProfit = data.winProfits[wins+1].winValue;
            expectedRawProfit = data.winProfits[wins+1].rawCurrency;            
        }
        else
        {
            expectedTotalValue = Mathf.Lerp(data.winProfits[wins+1].totalValue , data.winProfits[wins+2].totalValue , excess);
            expectedWinProfit = Mathf.Lerp(data.winProfits[wins+1].winValue , data.winProfits[wins+2].winValue , excess);
            expectedRawProfit = Mathf.Lerp(data.winProfits[wins+1].rawCurrency , data.winProfits[wins+2].rawCurrency , excess);
        }

        data.expectedTotalValue = expectedTotalValue;
        data.expectedWinProfit = expectedWinProfit;
        data.expectedRawCurrency = expectedRawProfit;
    }
}
