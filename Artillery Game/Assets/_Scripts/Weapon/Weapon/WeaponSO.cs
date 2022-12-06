using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Character/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Broadcasting To")]
    [SerializeField] WeaponEventsChannelSO weaponEvents;

    [SerializeField] enum FireMode{ None, Missile }

    [Header("Weapon Settings")] 
    [SerializeField] FireMode fireMode;
    public float fireRate = 0.3f;
    public FireModeMissile selectedFireMode;
    
    [Header("Projectiles")]
    public Projectile[] projectiles;


    private void OnEnable() 
    {
        switch (fireMode)
        {          
            case FireMode.Missile:
            selectedFireMode = new FireModeMissile();
            break;
        }
        
        selectedFireMode.weaponEvents = weaponEvents;
    }   


    public void Shoot(Vector3 position, Quaternion rotation, float power)
    {      
        selectedFireMode.Shoot(projectiles, position, rotation, power, fireRate);
    }
}
