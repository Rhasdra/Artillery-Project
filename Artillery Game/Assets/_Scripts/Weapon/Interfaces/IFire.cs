using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShoot
{
    void Shoot(GameObject[] proj, Vector3 position, Quaternion rotation, float power, float fireRate);
}
