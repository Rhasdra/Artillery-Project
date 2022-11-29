using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class textBox_Stats : MonoBehaviour
{
    public GlobalTables globalTables;
    public int i = 0;

    TextMeshProUGUI text;

    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText()
    {
        text.text = new string(
            "Profit Chance: " + globalTables.limitedDatas[i].profitChance + "\n" +
            "Infinite Chance: " + globalTables.limitedDatas[i].infiniteChance + "\n" + 
            "Expected Wins: " + globalTables.limitedDatas[i].limitedEvent.winrateTable.expectedWins + "\n" +
            "Total Value: " + globalTables.limitedDatas[i].expectedTotalValue + "\n" +
            "Win Profit: " + globalTables.limitedDatas[i].expectedWinProfit + "\n" +
            "Raw Currency: " + globalTables.limitedDatas[i].expectedRawCurrency + "\n"
            );
    }
}
