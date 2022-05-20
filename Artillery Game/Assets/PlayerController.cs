using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CharController[] charController;
    [SerializeField] int index;

    PlayerInputActions playerControls;

    InputAction movement;

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
    }

    private void OnDisable() 
    {
        movement.Disable();
        playerControls.Player.LongJump.Disable();
        playerControls.Player.BackFlip.Disable();
    }

    private void Update() 
    {
        MovementValueChanged(movement.ReadValue<Vector2>());
    }

    private void MovementValueChanged(Vector2 value)
    {
        charController[index].OnMovementValueChanged.Invoke(value);   
    }

    private void LongJump(InputAction.CallbackContext obj)
    {
        charController[index].OnLongJumpPress.Invoke();

        if(debug)
        {
            Debug.Log("LongJump Pressed");
        }
    }

    private void BackFlip(InputAction.CallbackContext obj)
    {
        charController[index].OnBackFlipPress.Invoke();

        if(debug)
        {
            Debug.Log("BackFlip Pressed");
        }
    }

    public void NextCharacter()
    {
        charController[index].EndTurn.Invoke();

        if ( index < charController.Length -1)
        {
            index++;
            charController[index].StartTurn.Invoke();
            return;
        }

        index = 0;
        charController[index].StartTurn.Invoke();
    }
}
