using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] enum FireMode{ None, Missile }
    [SerializeField] FireMode fireMode;
    
    public Projectile[] projectiles;
    public IFire projFire;

    public float fireRate = 0.3f;


    private void OnEnable() 
    {
        switch (fireMode)
        {
            case FireMode.None:
            gameObject.AddComponent<FireNone>();
            break;
            
            case FireMode.Missile:
            gameObject.AddComponent<FireMissile>();
            break;
        }

        projFire = GetComponent<IFire>();
    }   


    public void Fire(Vector3 position, Quaternion rotation, float power)
    {
        projFire.Fire(projectiles, position, rotation, power, fireRate);
    }

}