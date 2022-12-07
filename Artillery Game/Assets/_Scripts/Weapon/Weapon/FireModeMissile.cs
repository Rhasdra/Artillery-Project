using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireModeMissile : MonoBehaviour , IShoot
{
    public WeaponEventsChannelSO weaponEvents;
    public WeaponSO weaponSO;

    public void Shoot(GameObject[] proj, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        StartCoroutine(SpawnProjectile(proj, position, rotation, power, fireRate));
    }

    IEnumerator SpawnProjectile(GameObject[] proj, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        for (int i = 0; i < proj.Length; i++)
        {
            Projectile newProj = Instantiate(proj[i], position, rotation).GetComponent<Projectile>();
            newProj.ProjLaunch(power);

            yield return new WaitForSeconds(fireRate);
        }       
    }
}
