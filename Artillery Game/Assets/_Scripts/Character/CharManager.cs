using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharManager : MonoBehaviour
{
    public CharSO charInfo;

    [Header("Listening to")]
    [SerializeField] TurnsManagerEventsChannelSO turnsManagerEvents;
    [SerializeField] WeaponEventsChannelSO weaponEvents;

    [Header("Broadcasting to")]
    [SerializeField] CharManagerEventsChannelSO charManagerEvents;

    [Header("Toggle Components Between Turns")]
    [SerializeField] SweetSpotDisplay sweetSpotUI;
    [SerializeField] Movement movement;
    [SerializeField] Aiming aiming;
    [SerializeField] Weapons weapons;

    private void Awake() 
    {   
        movement = GetComponent<Movement>();
        aiming = GetComponent<Aiming>();
        weapons = GetComponent<Weapons>();
        sweetSpotUI = GetComponentInChildren<SweetSpotDisplay>();
    }

    public void StartListening()
    {
        turnsManagerEvents.StartTurn.OnEventRaised += StartTurn;
        turnsManagerEvents.EndTurn.OnEventRaised += EndTurn;

        weaponEvents.ShootEvent.OnEventRaised += RequestEndTurn;
    }

    public void StopListening()
    {
        turnsManagerEvents.StartTurn.OnEventRaised -= StartTurn;
        turnsManagerEvents.EndTurn.OnEventRaised -= EndTurn;

        weaponEvents.ShootEvent.OnEventRaised -= RequestEndTurn;
    }


    public void StartTurn()
    {
        movement.enabled = true;
        aiming.enabled = true;
        weapons.enabled = true;
        sweetSpotUI.Display(true);
    }

    public void EndTurn()
    {
        movement.enabled = false;
        aiming.enabled = false;
        weapons.enabled = false;
        sweetSpotUI.Display(false);

        StopListening();
    }

    public void RequestEndTurn()
    {
        //TODO
        //Check if all projectiles are destroyed
        //If yes, call the end turn event

        charManagerEvents.EndTurn.OnEventRaised.Invoke();
    }
}
