using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputActions.IPlayerActions
{
    PlayerInputActions playerInput;

    // Gameplay
    public event UnityAction<Vector2> MovementInputValueEvent = delegate { };
    public event UnityAction OnLongJumpPressEvent = delegate { };
    public event UnityAction OnBackFlipPressEvent = delegate { };

    public event UnityAction<int> PowerInputValueEvent = delegate { };
    public event UnityAction<int> PowerInputPressEvent = delegate { };
    public event UnityAction PowerInputHeldEvent = delegate { };
    public event UnityAction PowerInputCanceledEvent = delegate { };

    public event UnityAction<int> AimInputValueEvent = delegate { };
    public event UnityAction<int> AimInputPressEvent = delegate { };
    public event UnityAction AimInputHeldEvent = delegate { };
    public event UnityAction AimInputCanceledEvent = delegate { };

    public event UnityAction FirePressEvent = delegate { };
    public event UnityAction<int> ScrollWeaponPressEvent = delegate { };

    private void OnEnable() 
    {
        if (playerInput == null)
        {
        	playerInput = new PlayerInputActions();   
            playerInput.Player.SetCallbacks(this);
        }

        EnablePlayerInput();
        playerInput.Player.Move.Enable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("OnMove Called!" + context.ReadValue<Vector2>());

        if (MovementInputValueEvent != null)
        {
            MovementInputValueEvent.Invoke(context.ReadValue<Vector2>());
        }else{
            Debug.Log("OnMove is null");
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (FirePressEvent != null)
        {
            FirePressEvent.Invoke();
        }
    }

    public void OnLongJump(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnBackFlip(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnPowerChange(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnScrollWeapon(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnWeaponNumber(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    } 
    
    private void OnDisable() 
    {
        //DisableAllInput();
    }

    public void EnablePlayerInput()
    {
        playerInput.UI.Disable();
        playerInput.Player.Enable();        
    }

    public void EnableUIInput()
    {
        playerInput.Player.Disable();        
        playerInput.UI.Enable();
    }
}
