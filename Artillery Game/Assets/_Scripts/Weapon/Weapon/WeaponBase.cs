using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("Broadcasting To")]
    [SerializeField] WeaponEventsChannelSO weaponEvents;

    [SerializeField] enum FireMode{ None, Missile }

    [Header("Weapon Settings")] 
    [SerializeField] FireMode fireMode;
    public float fireRate = 0.3f;
    public FireModeMissile selectedFireMode;
    
    [Header("Projectiles")]
    public GameObject[] projectiles;


    private void OnEnable() 
    {
        switch (fireMode)
        {          
            case FireMode.Missile:
            gameObject.AddComponent<FireModeMissile>();
            break;
        }

        selectedFireMode = GetComponent<FireModeMissile>();
        //selectedFireMode.weaponEvents = weaponEvents;
    }   


    public void Shoot(Vector3 position, Quaternion rotation, float power)
    {      
        selectedFireMode.Shoot(projectiles, position, rotation, power, fireRate);
    }

}