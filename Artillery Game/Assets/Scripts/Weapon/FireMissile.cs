using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMissile : MonoBehaviour , IFire
{
    public void Fire(ProjectileBase[] proj, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        StartCoroutine(SpawnProjectile(proj, position, rotation, power, fireRate));
    }

    IEnumerator SpawnProjectile(ProjectileBase[] proj, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        for (int i = 0; i < proj.Length; i++)
        {
            ProjectileBase newProj = Instantiate(proj[i], position, rotation);
            newProj.ProjLaunch(power); 
            yield return new WaitForSeconds(fireRate);
        }
        
    }
}
