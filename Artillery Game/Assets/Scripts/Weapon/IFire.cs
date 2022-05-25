using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFire
{
    void Fire(ProjectileBase[] proj, Vector3 position, Quaternion rotation, float power, float fireRate);
}
