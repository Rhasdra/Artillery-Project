using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Character/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Weapon Settings")] 
    public FireMode fireMode;
    public Delay delayEnum;
    public float fireRate = 0.3f;
    public float deviation = 0f;
    [HideInInspector] public int delay = 3;
    
    public enum FireMode{ None, Missile }
    public enum Delay{ weak, strong, SS }
    
    [Header("Projectiles")]
    public GameObject[] projectiles;

    [Header("Trail")]
    public GameObject trail;
    
    private void OnEnable() 
    {
        switch (delayEnum)
            {
                case Delay.weak:
                delay = 3;
                break;

                case Delay.strong:
                delay = 4;
                break;

                case Delay.SS:
                delay = 7;
                break;
            }
    }
 
}
