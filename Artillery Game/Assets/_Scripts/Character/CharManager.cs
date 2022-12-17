using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharManager : MonoBehaviour
{
    public TeamSO team;
    public CharSO charInfo;

    [Header("Listening to")]
    [SerializeField] TurnsManagerEventsChannelSO turnsManagerEvents;
    [SerializeField] BattleManagerEventsChannelSO battleManagerEvents;
    [SerializeField] MovementEventsChannelSO movementEvents;
    [SerializeField] AimingEventsChannelSO aimingEvents;
    [SerializeField] WeaponEventsChannelSO weaponEvents;
    [SerializeField] HealthEventsChannelSO healthEvents;

    [Header("Broadcasting to")]
    [SerializeField] CharManagerEventsChannelSO charManagerEvents;

    [Header("Runtime Set")]
    [SerializeField] GameObjectRuntimeSet charactersRuntimeSet;

    //Toggles these Components in Between Turns
    Movement movement;
    Aiming aiming;
    SweetSpotDisplay sweetSpotUI;
    WeaponsManager weapons;
    [SerializeField] SpriteRenderer aimReticle;

    [Header("Information Summary")]
    public int health;
    public float power;
    public float angle;
    public int weaponIndex;
    public int delay;
    bool isTakingTurn;

    [Header("Show Debug Info")]
    [SerializeField] bool showDebugInfo;
    [SerializeField] GameObject debugTextPrefab;
    GameObject debugTextInstance;

    public UnityEvent Death;

    private void Awake() 
    {   
        movement = GetComponent<Movement>();
        aiming = GetComponent<Aiming>();
        weapons = GetComponent<WeaponsManager>();
        sweetSpotUI = GetComponentInChildren<SweetSpotDisplay>();
    }

    private void Start() 
    {
        if(showDebugInfo)
        {
            debugTextInstance = Instantiate(debugTextPrefab, transform.position, Quaternion.identity);
            debugTextInstance.GetComponent<DebugText>().Setup(this);
        }
    }

    private void OnEnable() 
    {
        charactersRuntimeSet.Add(this.gameObject);
    }

    public void StartListening()
    {
        turnsManagerEvents.StartTurn.OnEventRaised += StartTurn;
        turnsManagerEvents.EndTurn.OnEventRaised += EndTurn;

        weaponEvents.ShootEvent.OnEventRaised += RequestEndTurn;

        //Info events
        weaponEvents.WeaponChangeEvent.OnEventRaised += GetInfo;
        aimingEvents.AngleChangeEvent.OnEventRaised += GetAngleInfo;
        aimingEvents.PowerChangeEvent.OnEventRaised += GetPowerInfo;

        //Delay events
        movementEvents.ThresholdCrossedEvent.OnEventRaised += AddMovementDelay;
        weaponEvents.ShootDelayEvent.OnEventRaised += AddWeaponDelay;
    }

    public void StopListening()
    {
        turnsManagerEvents.StartTurn.OnEventRaised -= StartTurn;
        turnsManagerEvents.EndTurn.OnEventRaised -= EndTurn;

        weaponEvents.ShootEvent.OnEventRaised -= RequestEndTurn;

        //Info events
        weaponEvents.WeaponChangeEvent.OnEventRaised -= GetInfo;
        aimingEvents.AngleChangeEvent.OnEventRaised -= GetAngleInfo;
        aimingEvents.PowerChangeEvent.OnEventRaised -= GetPowerInfo;

        //Delay events
        movementEvents.ThresholdCrossedEvent.OnEventRaised -= AddMovementDelay;
        weaponEvents.ShootDelayEvent.OnEventRaised -= AddWeaponDelay;

        //Runtime Set Projectiles
        battleManagerEvents.EmptyProjectileList.OnEventRaised -= ConfirmEndTurn;
    }


    public void StartTurn()
    {
        isTakingTurn = true;

        movement.enabled = true;
        aiming.enabled = true;
        weapons.enabled = true;
        aimReticle.enabled = true;
        sweetSpotUI.Display(true);

        delay = 0;
    }

    public void EndTurn()
    {
        isTakingTurn = false;

        DisableComponents();

        GetInfo();
        StopListening();
    }

    public void DisableComponents()
    {
        movement.enabled = false;
        aiming.enabled = false;
        weapons.enabled = false;

        aimReticle.enabled = false;
        sweetSpotUI.Display(false);
    }

    public void RequestEndTurn() //Waits for all projectiles to be destroyed before ending turn
    {
        battleManagerEvents.EmptyProjectileList.OnEventRaised += ConfirmEndTurn;
       
        movement.enabled = false;
        aiming.enabled = false;
        weapons.enabled = false;
    }

    void ConfirmEndTurn() //All projectiles are gone, let's trigger the end of turn
    {
        StartCoroutine(EndTurnCoroutine());
    }

    IEnumerator EndTurnCoroutine()
    {
        battleManagerEvents.EmptyProjectileList.OnEventRaised -= ConfirmEndTurn;
    
        yield return new WaitForSeconds(1.5f);
    
        charManagerEvents.EndTurn.OnEventRaised.Invoke();
    }

    void GetInfo()
    {
        power = aiming.power;
        angle = aiming.angle;
        weaponIndex = weapons.index;
    }

    void GetWeaponIndexInfo(int value)
    {
        weaponIndex = value;
    }

    void GetAngleInfo(int value)
    {
        angle = value;
    }

    void GetPowerInfo(int value)
    {
        power = value;
    }

    void AddWeaponDelay(int value)
    {
        delay += value;
    }

    void AddMovementDelay()
    {
        delay += movement.moveDelay;
    }

    public void RequestDeath()
    {
        charManagerEvents.EndTurn.OnEventRaised += Die;
    }

    public void Die()
    {
        charManagerEvents.EndTurn.OnEventRaised -= Die;
        Death.Invoke();
        
        if(isTakingTurn)
        {
            EndTurn();
        }

        charactersRuntimeSet.Remove(this.gameObject);
        healthEvents.CharacterDeath.RaiseEvent(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
