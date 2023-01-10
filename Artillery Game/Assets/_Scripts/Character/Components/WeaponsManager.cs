using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponsManager : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] InputReader inputReader;

    [Header("Broadcasting To")]
    [SerializeField] WeaponEventsChannelSO weaponEvents;
    
    public UnityAction ShootEvent = delegate { };
    public UnityAction<int> ShootDelayEvent = delegate { };
    public UnityAction WeaponChangeEvent = delegate { };

    [Header("Settings")]
    [SerializeField] float spawnOffset = 0.75f;

    [Header("Information")]
    public GameObject[] weapons;
    public Weapon currentWeapon;
    public int index;
    [SerializeField] FireModeMissile fireMode;

    [Header("Trail")]
    [SerializeField] GameObject trailPrefab;

    Aiming aiming;

    public void Initialize() 
    {
        aiming = GetComponent<Aiming>();
        weapons = GetComponent<CharManager>().jobInfo.weapons;
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
        if (currentWeapon != null)
            Destroy(currentWeapon);
            
        currentWeapon = Instantiate(weapons[index], transform.position, Quaternion.identity, this.transform).GetComponent<Weapon>();
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
        WeaponChangeEvent.Invoke();

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

        currentWeapon.Shoot(spawnPosition, rotation, aiming.power);
        
        //Broadcast the event
        ShootEvent.Invoke();
        ShootDelayEvent.Invoke(currentWeapon.weaponSO.delay);

        weaponEvents.ShootDelayEvent.OnEventRaised(currentWeapon.weaponSO.delay);
        weaponEvents.ShootEvent.OnEventRaised();
        
    }

    private void OnDrawGizmos() 
    {
        Vector3 pos = new Vector3(transform.position.x + spawnOffset, transform.position.y, transform.position.z);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.05f);    
    }
}
