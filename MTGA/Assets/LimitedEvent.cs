using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LimitedEvent", menuName = "MTGA/LimitedEvent", order = 0)]
public class LimitedEvent : ScriptableObject 
{
    public int gems;
    public int gold;
    public float packsOpen;
    public WinrateTable winrateTable;
    public Win[] wins;
}

