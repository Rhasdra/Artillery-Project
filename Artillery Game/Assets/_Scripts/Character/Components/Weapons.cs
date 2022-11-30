using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapons : MonoBehaviour
{
    CharManager charManager;
    Aiming aiming;
    [SerializeField] float spawnOffset = 2f;

    public WeaponBase[] weapons;
    public WeaponBase currentWeapon;
    public int index;

    [SerializeField] IFire projFire;

    public UnityEvent ProjectileFired;
    public UnityEvent<int> SwappedWeapon;

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
        index = 0;
        GetWeapon();
    }

    public void GetWeapon()
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
 
        GetWeapon();

        SwappedWeapon.Invoke(index);
    }

    public void Fire()
    {
        ProjectileFired.Invoke();

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

    private void OnDrawGizmos() 
    {
        Vector3 pos = new Vector3(transform.position.x + spawnOffset, transform.position.y, transform.position.z);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.05f);    
    }
}
