using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Calculator : MonoBehaviour
{
    //public Profit profit;
    public float winRate = 0.5f;
    public GlobalTables globalTables;

    private void Start() 
    {
        foreach (var limitedEvent in globalTables.limitedEvents)
        {
            LimitedData data = new LimitedData();
            data.limitedEvent = limitedEvent;
            globalTables.limitedDatas.Add(data);
        }

        for (int i = 0; i < globalTables.limitedDatas.Count; i++)
        {
            // Generate profits table and assign to limitedData
            List<WinProfit> winProfits = new List<WinProfit>();
            Profit.CalcProfits(globalTables.limitedDatas[i].limitedEvent, winProfits);
            globalTables.limitedDatas[i].winProfits = winProfits; 

            //Calculate expected returns
            for (int j = 0; j < globalTables.limitedDatas.Count; j++)
            {
                Profit.CalcExpectedProfits(globalTables.limitedDatas[i]);   
            }                                 
        }

        foreach (var winrateTable in globalTables.winrateTables)
        {
            // Generate winrateTables
            List<WinChance> winChances = new List<WinChance>();
            Odds.CalculateOdds(winrateTable , winChances, winRate);
            winrateTable.winChances = winChances;
        }
    }
}
