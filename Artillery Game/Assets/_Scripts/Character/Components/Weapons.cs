using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapons : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] InputReader inputReader;

    [Header("Broadcasting To")]
    [SerializeField] WeaponEventsChannelSO weaponEvents;

    Aiming aiming;

    [SerializeField] float spawnOffset = 2f;

    public WeaponBase[] weapons;
    public WeaponBase currentWeapon;
    public int index;

    [SerializeField] IShoot projFire;

    private void Awake() 
    {
        aiming = GetComponent<Aiming>();
        weapons = GetComponent<CharManager>().charInfo.weapons;

        weapons[0].selectedFireMode = GetComponent<FireModeMissile>();
        projFire = weapons[0].selectedFireMode;
    }

    private void OnEnable() 
    {
        inputReader.ShootEvent += Shoot;
        inputReader.ScrollWeaponEvent += ScrollWeapon;
    }

    private void OnDisable()
    {
        inputReader.ShootEvent -= Shoot;
        inputReader.ScrollWeaponEvent -= ScrollWeapon;
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

        //Broadcast the event
        weaponEvents.WeaponChangeEvent.OnEventRaised(currentWeapon.gameObject);
    }

    public void Shoot()
    {
        Vector3 origin = aiming.shootingAxis.position;
        float flip = aiming.shootingAxis.parent.localScale.x;
        Vector3 spawnPosition = origin + (aiming.shootingAxis.right * spawnOffset * flip);
        Quaternion rotation = aiming.shootingAxis.rotation;

        if (flip == -1)
        {
        rotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z + 180f);
        }

        currentWeapon.Shoot(spawnPosition, rotation, aiming.power);
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
