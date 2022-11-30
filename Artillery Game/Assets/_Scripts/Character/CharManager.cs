using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharManager : MonoBehaviour
{
    public bool isMyTurn = false;
    [SerializeField] bool debug = false;
    public CharSO charInfo;

    public UnityEvent OnLongJumpPress;
    public UnityEvent OnBackFlipPress;
    public UnityEvent<Vector2> MovementInputValue;

    public UnityEvent<float> PowerInputValue;
    public UnityEvent<float> PowerInputPress;
    public UnityEvent PowerInputHeld;
    public UnityEvent PowerInputCanceled;

    public UnityEvent<float> AimInputValue;
    public UnityEvent<float> AimInputPress;
    public UnityEvent AimInputHeld;
    public UnityEvent AimInputCanceled;

    public UnityEvent FirePress;
    public UnityEvent<float> ScrollWeaponPress;

    public UnityEvent StartTurn;
    public UnityEvent EndTurn;

    private void Awake() 
    {
    }

    private void OnEnable() 
    {
        StartTurn.AddListener(SetIsMyTurnOn);
        EndTurn.AddListener(SetIsMyTurnOff);
    }

    private void OnDisable() 
    {
        StartTurn.RemoveListener(SetIsMyTurnOn);
        EndTurn.RemoveListener(SetIsMyTurnOff);
    }

    void SetIsMyTurnOn()
    {
        isMyTurn = true;

        if(debug)
        Debug.Log("SetIsMyTurnOn");
    }

    void SetIsMyTurnOff()
    {
        isMyTurn = false;

        if(debug)
        Debug.Log("SetIsMyTurnOff");
    }

}
