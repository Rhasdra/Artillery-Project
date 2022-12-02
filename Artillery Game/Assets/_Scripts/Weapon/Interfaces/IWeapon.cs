using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWeapon : MonoBehaviour
{
    virtual public void Fire (IShoot fire, Projectile[] projectiles, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        fire.Shoot(projectiles, position, rotation, power, fireRate);
    }

}
