using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/Data")]
public class CharDataSO : ScriptableObject
{
    public string CharName;
    
    public JobSO Job;
    public int[] WeaponsIndex;

    [Header("Statistics")]
    public float damageDealt;

    public void Init(JobSO job) 
    {
        Job = job;
        WeaponsIndex = new int[Job.weapons.Length];
    }
}
