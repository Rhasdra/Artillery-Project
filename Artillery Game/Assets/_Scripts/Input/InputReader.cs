using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInputActions.IGameplayActions, GameInputActions.IMenuActions
{
    GameInputActions playerInput;

    // Gameplay
    public event UnityAction<float> MoveEvent = delegate { };
    public event UnityAction<Vector2> LookEvent = delegate { };
    public event UnityAction ShootEvent = delegate { };
    public event UnityAction LongJumpEvent = delegate { };
    public event UnityAction BackFlipEvent = delegate { };

    public event UnityAction<float> PowerValueEvent = delegate { };
    public event UnityAction PowerPressEvent = delegate { };
    public event UnityAction PowerHeldEvent = delegate { };
    public event UnityAction PowerCanceledEvent = delegate { };

    public event UnityAction<float> AimValueEvent = delegate { };
    public event UnityAction AimPressEvent = delegate { };
    public event UnityAction AimHeldEvent = delegate { };
    public event UnityAction AimCanceledEvent = delegate { };

    public event UnityAction<float> ScrollWeaponEvent = delegate { };
    public event UnityAction<float> WeaponNumberEvent = delegate { };

    public event UnityAction<float> ZoomEvent = delegate { };

    public event UnityAction<Vector2> MousePositionEvent = delegate { };

    public event UnityAction PanPressEvent = delegate { };
    public event UnityAction PanCanceledEvent = delegate { };

    public event UnityAction PauseGameEvent = delegate { };

    //Menu


    private void OnEnable() 
    {
        if (playerInput == null)
        {
        	playerInput = new GameInputActions();   
            playerInput.Gameplay.SetCallbacks(this);
            playerInput.Menu.SetCallbacks(this);
        }

        EnablePlayerInput();
    }
    
    private void OnDisable() 
    {
        DisableAllInput();
    }

    public void EnablePlayerInput()
    {
        playerInput.Menu.Disable();
        playerInput.Gameplay.Enable();        
    }

    public void EnableMenuInput()
    {
        playerInput.Gameplay.Disable();        
        playerInput.Menu.Enable();
    }

    public void DisableAllInput()
    {
        playerInput.Gameplay.Disable();        
        playerInput.Menu.Disable();
    }

    #region Gameplay
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<float>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        ShootEvent.Invoke();
    }

    public void OnLongJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        LongJumpEvent.Invoke();
    }

    public void OnBackFlip(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        BackFlipEvent.Invoke();
    }

    public void OnPowerChange(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                PowerValueEvent.Invoke(context.ReadValue<float>());
                PowerPressEvent.Invoke();
                break;
            case InputActionPhase.Performed:
                PowerHeldEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                PowerCanceledEvent.Invoke();
                break;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                AimValueEvent.Invoke(context.ReadValue<float>());
                AimPressEvent.Invoke();
                break;
            case InputActionPhase.Performed:
                AimHeldEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                AimCanceledEvent.Invoke();
                break;
        }
    }

    public void OnScrollWeapon(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        ScrollWeaponEvent.Invoke(context.ReadValue<float>());
    }

    public void OnWeaponNumber(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        WeaponNumberEvent.Invoke(context.ReadValue<float>());
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        ZoomEvent.Invoke(context.ReadValue<float>());
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        MousePositionEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPanDrag(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                PanPressEvent.Invoke();
                break;
            // case InputActionPhase.Performed:
            //     PowerHeldEvent.Invoke();
            //     break;
            case InputActionPhase.Canceled:
                PanCanceledEvent.Invoke();
                break;
        }
    }
    
    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        PauseGameEvent.Invoke();    
    }

    #endregion

    #region Menu
    public void OnNavigate(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }



    #endregion
}
