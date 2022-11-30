using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharManager))]
public class Aiming : MonoBehaviour
{
    CharManager charManager;
    CharSO charInfo;
    
    public Transform shootingAxis;

    
    [Header("Power")]
    public float power = 50f;
    float powerInput;
    bool powerInputHold;
    float pwrAcc = 1;
    
    [Header("Aim")]
    public float angle;
    [SerializeField] float sweetSpotMax = 0f;
    [SerializeField] float sweetSpotMin = 0f;
    public bool sweetSpot = false;
    float aim = 135f;
    float aimInput;
    bool aimInputHold;
    float aimAcc = 1;

    [Header("Debug")]
    [SerializeField] bool debug = false;

    private void Awake() 
    {
        charManager = GetComponent<CharManager>();
        charInfo = charManager.charInfo;
        
        sweetSpotMax = charInfo.sweetSpotAngleMax;
        sweetSpotMin = charInfo.sweetSpotAngleMin;
    }

    private void OnEnable() 
    {
        charManager.PowerInputValue.AddListener(GetPowerInputValue);
        charManager.PowerInputPress.AddListener(PowerInputPress);
        charManager.PowerInputHeld.AddListener(PowerInputHeld);
        charManager.PowerInputCanceled.AddListener(PowerInputCanceled);

        charManager.AimInputValue.AddListener(GetAimInputValue);
        charManager.AimInputPress.AddListener(AimInputPress);
        charManager.AimInputHeld.AddListener(AimInputHeld);
        charManager.AimInputCanceled.AddListener(AimInputCanceled);
    }

    private void OnDisable() 
    {
        charManager.PowerInputValue.RemoveListener(GetPowerInputValue);
        charManager.PowerInputPress.RemoveListener(PowerInputPress);
        charManager.PowerInputHeld.RemoveListener(PowerInputHeld);
        charManager.PowerInputCanceled.RemoveListener(PowerInputCanceled);

        charManager.AimInputValue.RemoveListener(GetAimInputValue);
        charManager.AimInputPress.RemoveListener(AimInputPress);
        charManager.AimInputHeld.RemoveListener(AimInputHeld);
        charManager.AimInputCanceled.RemoveListener(AimInputCanceled);
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

        power += input * 10f * pwrAcc * Time.deltaTime;
        pwrAcc = Mathf.Clamp(pwrAcc + (pwrAcc * Time.deltaTime * 1.5f) , 0, 20f);
    }

    private void PowerInputPress(float input)
    {
        power += input * ( 1 - (power % 1) );

        if (debug)
        Debug.Log("PowerInputPress " + powerInput);
    }

    private void PowerInputHeld()
    {
        powerInputHold = true;
    }

    private void PowerInputCanceled()
    {
        pwrAcc = 1f;
        power = Mathf.RoundToInt(power);
        powerInputHold = false;
    }

    private void PowerClamp(float oldPower)
    {
        power = Mathf.Clamp(oldPower, 0 , 100);
    }
    #endregion


    #region Aim
    private void GetAimInputValue(float inputValue)
    {
        aimInput = inputValue;
    }

    private void AdjustAimValue(float input)
    {
        if (aimInputHold == false)
        return;

        aim += input * 10f * aimAcc * Time.deltaTime;
        aimAcc = Mathf.Clamp(aimAcc + (aimAcc * Time.deltaTime * 1.5f) , 0, 20f);
    }

    private void AimInputPress(float input)
    {
        aim += input * ( 1 - (power % 1) );

        if (debug)
        Debug.Log("PowerInputPress " + powerInput);
    }

    private void AimInputHeld()
    {
        aimInputHold = true;
    }
    
    private void AimInputCanceled()
    {
        aimAcc = 1f;
        aim = Mathf.RoundToInt(aim);
        aimInputHold = false;
    }

    private void AimClamp(float oldAim)
    {
        aim = Mathf.Clamp(oldAim, 0 , 180);
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
    }
    #endregion

    ///// SHOOTING AXIS /////
    void RotateShootAxis (float angle)
    {
        shootingAxis.localRotation = Quaternion.Euler (0f, 0f, (Mathf.RoundToInt(aim - 90f)));
    }

}
