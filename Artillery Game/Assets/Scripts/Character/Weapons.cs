using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    CharManager charManager;
    Aiming aiming;
    [SerializeField] float spawnOffset = 2f;

    [SerializeField] WeaponBase[] weapons;
    [SerializeField] WeaponBase currentWeapon;
    [SerializeField] int index;

    [SerializeField] IFire projFire;

    private void Awake() 
    {
        charManager = GetComponent<CharManager>();
        aiming = GetComponent<Aiming>();
        weapons = charManager.charInfo.weapons;

        weapons[0].projFire = GetComponent<FireMissile>();
        projFire = weapons[0].projFire;
    }

    private void OnEnable() 
    {
        charManager.FirePress.AddListener(Fire);
        charManager.ScrollWeaponPress.AddListener(ScrollWeapon);
    }

    private void OnDisable()
    {
        charManager.FirePress.RemoveListener(Fire);
    }

    private void Start() 
    {
        GetWeapon(0);
    }

    void GetWeapon(int index)
    {
        Object.Destroy(currentWeapon?.gameObject);

        GameObject wpn = Instantiate(weapons[index].gameObject, aiming.shootingAxis.position, aiming.shootingAxis.rotation, aiming.shootingAxis);
        currentWeapon = wpn.GetComponent<WeaponBase>();
    }

    void ScrollWeapon(float value)
    {
        index += (int)value;

        if(index > weapons.Length -1)
        index = 0;
        else if(index < 0)
        index = weapons.Length -1;
 
        GetWeapon(index);
    }

    public void Fire()
    {
        Vector3 origin = aiming.shootingAxis.position;
        float flip = aiming.shootingAxis.parent.localScale.x;
        Vector3 spawnPosition = origin + (aiming.shootingAxis.right * spawnOffset * flip);
        Quaternion rotation = aiming.shootingAxis.rotation;

        if (flip == -1)
        {
        rotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z + 180f);
        }

        currentWeapon.Fire(spawnPosition, rotation, aiming.power);
        PassTurn();
    }

    void PassTurn()
    {
        TurnsManager.NextTurn.Invoke();
    }
}
