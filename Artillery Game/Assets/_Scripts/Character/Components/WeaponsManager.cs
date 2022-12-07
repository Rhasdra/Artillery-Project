using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] InputReader inputReader;

    [Header("Broadcasting To")]
    [SerializeField] WeaponEventsChannelSO weaponEvents;
    Aiming aiming;

    [Header("Settings")]
    [SerializeField] float spawnOffset = 0.75f;

    [Header("Information")]
    public WeaponSO[] weapons;
    public WeaponSO currentWeapon;
    public int index;
    [SerializeField] FireModeMissile fireMode;

    [Header("Trail")]
    [SerializeField] GameObject trailPrefab;


    private void Awake() 
    {
        aiming = GetComponent<Aiming>();
        weapons = GetComponent<CharManager>().charInfo.weaponsSO;
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
        currentWeapon = weapons[index];
        GetFireMode();
    }

    void ScrollWeapon(float value)
    {
        index += (int)value;

        if(index > weapons.Length -1)
        index = 0;
        else if(index < 0)
        index = weapons.Length -1;
 
        GetWeapon();

        //Broadcast Void and Int events
        weaponEvents.WeaponChangeEvent.OnEventRaised.Invoke();
        weaponEvents.WeaponChangeIndexEvent.OnEventRaised.Invoke(index);
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

        fireMode.Shoot(currentWeapon.projectiles, spawnPosition, rotation, aiming.power, currentWeapon.fireRate);
        
        //Broadcast the event
        weaponEvents.ShootDelayEvent.OnEventRaised(currentWeapon.delay);
        weaponEvents.ShootEvent.OnEventRaised();
        
    }

    private void OnDrawGizmos() 
    {
        Vector3 pos = new Vector3(transform.position.x + spawnOffset, transform.position.y, transform.position.z);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.05f);    
    }

    void GetFireMode()
    {
        if(fireMode != null)
            Destroy(GetComponent(typeof(IShoot)));

        switch (currentWeapon.fireMode)
        {
            case WeaponSO.FireMode.Missile:
                fireMode = this.gameObject.AddComponent<FireModeMissile>();
                fireMode.weaponEvents = weaponEvents;
                fireMode.weaponSO = currentWeapon;
                break;
        }
    }
}
