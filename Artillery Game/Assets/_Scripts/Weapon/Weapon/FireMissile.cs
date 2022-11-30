using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMissile : MonoBehaviour , IFire
{
    public void Fire(Projectile[] proj, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        StartCoroutine(SpawnProjectile(proj, position, rotation, power, fireRate));
    }

    IEnumerator SpawnProjectile(Projectile[] proj, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        for (int i = 0; i < proj.Length; i++)
        {
            Projectile newProj = Instantiate(proj[i], position, rotation);
            newProj.ProjLaunch(power);
            yield return new WaitForSeconds(fireRate);
        }
        
    }
}
