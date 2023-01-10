using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Data")] 
    public WeaponSO weaponSO;

    public virtual void Shoot(Vector3 position, Quaternion rotation, float power)
    {
        StartCoroutine(SpawnProjectile(weaponSO.projectiles, position, rotation, power, weaponSO.fireRate));
    }

    IEnumerator SpawnProjectile(GameObject[] projectiles, Vector3 position, Quaternion rotation, float power, float fireRate)
    {
        bool firstOne = true;

        foreach (var proj in projectiles)
        {
            float size = proj.GetComponent<CapsuleCollider2D>().size.x / 2;
            Vector3 offset = new Vector3(position.x + size, position.y, position.z);

            if(firstOne == false)
            {
                float random = Random.Range(-weaponSO.deviation, weaponSO.deviation);
                rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z + random);
            }

            ProjectileManager newProj = Instantiate(proj, position, rotation).GetComponent<ProjectileManager>();
            newProj.RequestLaunch(power);

            firstOne = false;
            yield return new WaitForSeconds(fireRate);
        }      
    }
}
