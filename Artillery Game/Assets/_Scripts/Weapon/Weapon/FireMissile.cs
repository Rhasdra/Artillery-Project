using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireModeMissile : MonoBehaviour , IShoot
{
    public WeaponEventsChannelSO weaponEvents;

    public void Shoot(Projectile[] proj, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        StartCoroutine(SpawnProjectile(proj, position, rotation, power, fireRate));
    }

    IEnumerator SpawnProjectile(Projectile[] proj, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        for (int i = 0; i < proj.Length; i++)
        {
            Projectile newProj = Instantiate(proj[i], position, rotation);
            newProj.ProjLaunch(power);

            //Broadcast the event
            weaponEvents.ShootEvent.OnEventRaised(proj[i].gameObject, position, rotation);

            yield return new WaitForSeconds(fireRate);
        }
        
    }
}
