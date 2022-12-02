using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShoot
{
    void Shoot(Projectile[] proj, Vector3 position, Quaternion rotation, float power, float fireRate);
}
