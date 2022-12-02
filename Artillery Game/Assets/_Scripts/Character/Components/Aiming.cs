using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharManager))]
public class Aiming : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] InputReader inputReader;

    [Header("Broadcasting To")]
    [SerializeField] AimingEventsChannelSO aimingEvents;

    CharSO charInfo;
    
    [Header("References")]
    public Transform shootingAxis;

    [Header("Power")]
    public float power = 50f;
    float powerInput;
    bool powerInputHold;
    
    [Header("Aim")]
    public float angle;
    [SerializeField] float sweetSpotMax = 0f;
    [SerializeField] float sweetSpotMin = 0f;
    public bool sweetSpot = false;
    float aim = 135f;
    float aimInput;
    bool aimInputHold;

    [Header("Configs")]
    [SerializeField] float pwrAcceleration = 1;
    [SerializeField] float aimAcceleration = 1;

    [Header("Debug")]
    [SerializeField] bool debug = false;

    private void Awake() 
    {
        charInfo = GetComponent<CharManager>().charInfo;
        
        sweetSpotMax = charInfo.sweetSpotAngleMax;
        sweetSpotMin = charInfo.sweetSpotAngleMin;
    }

    private void OnEnable() 
    {
        inputReader.PowerValueEvent += GetPowerInputValue;
        inputReader.PowerPressEvent += PowerInputPress;
        inputReader.PowerHeldEvent += PowerInputHeld;
        inputReader.PowerCanceledEvent += PowerInputCanceled;

        inputReader.AimValueEvent += GetAimInputValue;
        inputReader.AimPressEvent += AimInputPress;
        inputReader.AimHeldEvent += AimInputHeld;
        inputReader.AimCanceledEvent += AimInputCanceled;
    }

    private void OnDisable() 
    {
        inputReader.PowerValueEvent -= GetPowerInputValue;
        inputReader.PowerPressEvent -= PowerInputPress;
        inputReader.PowerHeldEvent -= PowerInputHeld;
        inputReader.PowerCanceledEvent -= PowerInputCanceled;

        inputReader.AimValueEvent -= GetAimInputValue;
        inputReader.AimPressEvent -= AimInputPress;
        inputReader.AimHeldEvent -= AimInputHeld;
        inputReader.AimCanceledEvent -= AimInputCanceled;
    }

    private void Update() 
    {
        AdjustPowerValue(powerInput);
        PowerClamp(power);

        AdjustAimValue(aimInput);
        AimClamp(aim);

        GetSweetSpot();
        GetAngle();

        RotateShootAxis(aim);
    }


    #region Power
    private void GetPowerInputValue(float inputValue)
    {
        powerInput = inputValue;
    }

    private void AdjustPowerValue(float input)
    {
        if (powerInputHold == false)
        return;

        power += input * 10f * pwrAcceleration * Time.deltaTime;
        pwrAcceleration = Mathf.Clamp(pwrAcceleration + (pwrAcceleration * Time.deltaTime * 1.5f) , 0, 20f);
    }

    private void PowerInputPress()
    {
        power += powerInput * ( 1 - (power % 1) );

        if (debug)
        Debug.Log("PowerInputPress " + powerInput);
    }

    private void PowerInputHeld()
    {
        powerInputHold = true;
    }

    private void PowerInputCanceled()
    {
        pwrAcceleration = 1f;
        power = Mathf.RoundToInt(power);
        powerInputHold = false;
    }

    private void PowerClamp(float oldPower)
    {
        power = Mathf.Clamp(oldPower, 0 , 100);

        //Broadcast the event
        aimingEvents?.PowerChangeEvent.RaiseEvent((int)power);
    }
    #endregion


    #region Aim
    private void GetAimInputValue(float inputValue)
    {
        aimInput = inputValue;
    }

    private void AdjustAimValue(float value)
    {
        if (aimInputHold == false)
        return;

        aim += value * 10f * aimAcceleration * Time.deltaTime;
        aimAcceleration = Mathf.Clamp(aimAcceleration + (aimAcceleration * Time.deltaTime * 1.5f) , 0, 20f);
    }

    private void AimInputPress()
    {
        aim += aimInput * ( 1 - (power % 1) );

        if (debug)
        Debug.Log("PowerInputPress " + powerInput);
    }

    private void AimInputHeld()
    {
        aimInputHold = true;
    }
    
    private void AimInputCanceled()
    {
        aimAcceleration = 1f;
        aim = Mathf.RoundToInt(aim);
        aimInputHold = false;
    }

    private void AimClamp(float oldAim)
    {
        aim = Mathf.Clamp(oldAim, 0 , 180);

        //Broadcast the event
        aimingEvents?.PowerChangeEvent.RaiseEvent((int)aim);
    }

    private void GetAngle()
    {
        angle = Mathf.RoundToInt(Vector2.Angle(shootingAxis.up, Vector2.up));
        
        if(angle > 90)
        angle = 180 - angle;

        if(shootingAxis.rotation.eulerAngles.z > 180)
        angle = -angle;

        angle = angle * transform.localScale.x;        
    }

    private void GetSweetSpot()
    {
        float ssAim = aim - 90;

        if ( ssAim >= sweetSpotMin && ssAim <= sweetSpotMax)
        sweetSpot = true;
        else
        sweetSpot = false;

        //Broadcast the event
        aimingEvents?.SweetSpotEvent.RaiseEvent(sweetSpot);
    }
    #endregion

    ///// SHOOTING AXIS /////
    void RotateShootAxis (float angle)
    {
        shootingAxis.localRotation = Quaternion.Euler (0f, 0f, (Mathf.RoundToInt(aim - 90f)));
    }

}
