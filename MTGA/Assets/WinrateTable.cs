using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WinrateTable", menuName = "MTGA/WinrateTable", order = 0)]
public class WinrateTable : ScriptableObject 
{
    public int wins;
    public int maxLosses;
    public float expectedWins;
    public List<WinChance> winChances;
}

