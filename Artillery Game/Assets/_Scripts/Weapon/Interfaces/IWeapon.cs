using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWeapon : MonoBehaviour
{
    virtual public void Fire (IFire fire, Projectile[] projectiles, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        fire.Fire(projectiles, position, rotation, power, fireRate);
    }

}
