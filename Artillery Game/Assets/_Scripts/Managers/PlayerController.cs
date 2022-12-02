using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("ListeningTo")]
    [SerializeField] InputReader InputReader;

    [Header("Debug")]
    public bool debugMove = false;
    public bool debug = false;

    private void Awake() 
    {
    }

    private void OnEnable() 
    {

    }

    // private void OnDisable() 
    // {
    //     movement.Disable();
    //     playerControls.Player.LongJump.Disable();
    //     playerControls.Player.BackFlip.Disable();

    //     power.Disable();
    //     aim.Disable();
    //     fire.Disable();
    //     scrollWeapon.Disable();
    // }

    // private void Update() 
    // {
    //     MovementInputValue(movement.ReadValue<Vector2>());
    //     PowerInputValue(power.ReadValue<float>());
    //     AimInputValue(aim.ReadValue<float>());
    // }
}
