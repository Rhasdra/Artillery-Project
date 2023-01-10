using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Team/Team")]
public class TeamSO : ScriptableObject
{
    public string Name;
    public Color color;

    public List<CharDataSO> charDatas;
}
