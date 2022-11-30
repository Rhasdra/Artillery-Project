using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions playerControls;

    InputAction movement;
    InputAction power;
    InputAction aim;
    InputAction fire;
    InputAction scrollWeapon;

    [Header("Debug")]
    public bool debugMove = false;
    public bool debug = false;

    private void Awake() 
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable() 
    {
        movement = playerControls.Player.Move;
        movement.Enable();
        playerControls.Player.LongJump.performed += LongJump;
        playerControls.Player.LongJump.Enable();
        playerControls.Player.BackFlip.performed += BackFlip;
        playerControls.Player.BackFlip.Enable();
        
        power = playerControls.Player.PowerChange;
        power.Enable();
        power.started += PowerInputPress;
        power.performed += PowerInputHeld;
        power.canceled += PowerInputCanceled;

        aim = playerControls.Player.Aim;
        aim.Enable();
        aim.started += AimInputPress;
        aim.performed += AimInputHeld;
        aim.canceled += AimInputCanceled;

        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.started += FirePress;

        scrollWeapon = playerControls.Player.ScrollWeapon;
        scrollWeapon.Enable();
        scrollWeapon.started += ScrollWeaponPress;

    }

    private void OnDisable() 
    {
        movement.Disable();
        playerControls.Player.LongJump.Disable();
        playerControls.Player.BackFlip.Disable();

        power.Disable();
        aim.Disable();
        fire.Disable();
        scrollWeapon.Disable();
    }

    private void Update() 
    {
        MovementInputValue(movement.ReadValue<Vector2>());
        PowerInputValue(power.ReadValue<float>());
        AimInputValue(aim.ReadValue<float>());
    }

    #region Movement
    private void MovementInputValue(Vector2 value)
    {
        TurnsManager.currentChar?.MovementInputValue.Invoke(value);   
    }

    private void LongJump(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.OnLongJumpPress.Invoke();

        if(debug)
        {
            Debug.Log("LongJump Pressed");
        }
    }

    private void BackFlip(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.OnBackFlipPress.Invoke();

        if(debug)
        {
            Debug.Log("BackFlip Pressed");
        }
    }
    #endregion

    #region Power
    private void PowerInputValue(float value)
    {
        TurnsManager.currentChar?.PowerInputValue.Invoke(value);
    }

    private void PowerInputPress(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.PowerInputPress.Invoke(power.ReadValue<float>());
    }

    private void PowerInputHeld(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.PowerInputHeld.Invoke();
    }

    private void PowerInputCanceled(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.PowerInputCanceled.Invoke();
    }
    #endregion

    #region Aim
    private void AimInputValue(float value)
    {
        TurnsManager.currentChar?.AimInputValue.Invoke(value);
    }

    private void AimInputPress(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.AimInputPress.Invoke(aim.ReadValue<float>());
    }

    private void AimInputHeld(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.AimInputHeld.Invoke();
    }

    private void AimInputCanceled(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.AimInputCanceled.Invoke();
    }
    #endregion

    private void FirePress(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.FirePress.Invoke();
    }

    private void ScrollWeaponPress(InputAction.CallbackContext obj)
    {
        TurnsManager.currentChar?.ScrollWeaponPress.Invoke(scrollWeapon.ReadValue<float>());
    }
}
