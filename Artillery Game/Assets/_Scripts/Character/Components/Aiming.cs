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
        [Tooltip("Current global angle. Handles the numbers shown in the UI.")]
    public float angle;
        [Tooltip("Current localAngle that the character is aiming at. Inspector value -90 is the starting angle.")]
    [SerializeField] float aim = 135f;
    float sweetSpotMax = 0f;
    float sweetSpotMin = 0f;
    public bool sweetSpot = false;
    float aimInput;
    bool aimInputHold;

    [Header("Configs")]
        [Tooltip("How fast the power changes when input is held.")]
    [SerializeField] float pwrAcceleration = 1;
        [Tooltip("How fast the aim changes when input is held.")]
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

        //If ending turn with button held it does not reset
        //Manual reset:
        aimInput = 0;
        aimInputHold = false;
        powerInput = 0;
        powerInputHold = false;
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
    private void GetPowerInputValue(float inputValue) //Reads and stores the input value from the inputReader. Maybe it can be deleted or transformed from void into float.
    {
        powerInput = inputValue;
    }

    private void AdjustPowerValue(float input) //Math to change the power value when HOLDING the button
    {
        if (powerInputHold == false)
        return;

        power += input * 10f * pwrAcceleration * Time.deltaTime;
        pwrAcceleration = Mathf.Clamp(pwrAcceleration + (pwrAcceleration * Time.deltaTime * 1.5f) , 0, 20f);
    }

    private void PowerInputPress() //Math to change the power value when TAPPING the button. Should always increase by one each tap
    {
        power += powerInput * ( 1 - (power % 1) );

        if (debug)
        Debug.Log("PowerInputPress " + powerInput);
    }

    private void PowerInputHeld() //Maybe it can be cut. Right now AdjustPowerValue is triggering on Update(), should be able to remove this if it's triggering on the InputReader event.
    {
        powerInputHold = true;
    }

    private void PowerInputCanceled() //Rounds the number and resets the acceleration multiplier when releasing the button
    {
        pwrAcceleration = 1f;
        power = Mathf.RoundToInt(power);
        powerInputHold = false;
    }

    private void PowerClamp(float oldPower) //Insures the values won't go out of bounds. It also broadcasts the value after clamping
    {
        power = Mathf.Clamp(oldPower, 0 , 100);

        //Broadcast the event
        aimingEvents?.PowerChangeEvent.RaiseEvent((int)power);
    }
    #endregion


    #region Aim
    private void GetAimInputValue(float inputValue) //Reads and stores the input value from the inputReader. Maybe it can be deleted or transformed from void into float.
    {
        aimInput = inputValue;
    }

    private void AdjustAimValue(float value) //Math to change the aim value when HOLDING the button
    {
        if (aimInputHold == false)
        return;

        aim += value * 10f * aimAcceleration * Time.deltaTime;
        aimAcceleration = Mathf.Clamp(aimAcceleration + (aimAcceleration * Time.deltaTime * 1.5f) , 0, 20f);
    }

    private void AimInputPress() //Math to change the aim value when TAPPING the button. Should always increase by one each tap
    {
        aim += aimInput * ( 1 - (power % 1) );

        if (debug)
        Debug.Log("PowerInputPress " + powerInput);
    }

    private void AimInputHeld() //Maybe it can be cut. Right now AdjustPowerValue is triggering on Update(), should be able to remove this if it's triggering on the InputReader event.
    {
        aimInputHold = true;
    }
    
    private void AimInputCanceled() //Rounds the number and resets the acceleration multiplier when releasing the button
    {
        aimAcceleration = 1f;
        aim = Mathf.RoundToInt(aim);
        aimInputHold = false;
    }

    private void AimClamp(float oldAim) //Insures the values won't go out of bounds. It also broadcasts the value after clamping
    {
        aim = Mathf.Clamp(oldAim, 0 , 180);

        //Broadcast the event
        aimingEvents?.AngleChangeEvent.RaiseEvent((int)aim);
    }

    private void GetAngle() //Calculates the aim value in relation to the world. Gets the value that is displayed by the UI
    {
        angle = Mathf.RoundToInt(Vector2.Angle(shootingAxis.up, Vector2.up));
        
        if(angle > 90)
        angle = 180 - angle;

        if(shootingAxis.rotation.eulerAngles.z > 180)
        angle = -angle;

        angle = angle * transform.localScale.x;        
    }

    private void GetSweetSpot() //Is the current aim inside the sweetspot range? THIS ONE SHOULD NOT BE ON UPDATE(), MUST REWRITE
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
        shootingAxis.localRotation = Quaternion.Euler (0f, 0f, (aim - 90f));
    }

}
