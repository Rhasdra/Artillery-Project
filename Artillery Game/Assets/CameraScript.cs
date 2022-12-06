using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] MovementEventsChannelSO moveEventsChannel;
    [SerializeField] WeaponEventsChannelSO weaponEventsChannel;
    [SerializeField] ProjectileEventsChannelSO projectileEventsChannel; 

    CinemachineVirtualCamera cam;
    CinemachineFramingTransposer transposer;
    bool canSwitchTarget = true;

    [Header("Settings")]
    [Tooltip("Events that the player doesn't control have a cooldown timer before triggering a camera transition.")]
    [SerializeField] float switchTimerSeconds = 0.5f;

    private void Awake() 
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void OnEnable() 
    {
        moveEventsChannel.MoveStartEvent.OnEventRaised += GetNewTarget;
        moveEventsChannel.MoveStartEvent.OnEventRaised += FrameLower;

        moveEventsChannel.LongJumpEvent.OnEventRaised += GetNewTarget;
        moveEventsChannel.LongJumpEvent.OnEventRaised += FrameLower;
        moveEventsChannel.BackFlipJumpEvent.OnEventRaised += GetNewTarget;
        moveEventsChannel.BackFlipJumpEvent.OnEventRaised += FrameLower;

        projectileEventsChannel.SpawnEvent.OnEventRaised += GetNewTarget;
        projectileEventsChannel.SpawnEvent.OnEventRaised += FrameCenter;

        projectileEventsChannel.HitEvent.OnEventRaised += GetNewTarget;
    }

    void SwitchTarget(Transform target)
    {
        if(target.transform != cam.Follow)
        {
            StartCoroutine(TickTimer());
            cam.Follow = target.transform;
        }
    }

    public void GetNewTarget (Transform target)
    {
        SwitchTarget(target);
    }
    public void GetNewTarget (GameObject target)
    {
        SwitchTarget(target.transform);
    }
    public void GetNewTarget (GameObject target, Vector3 pos, Quaternion rotation)
    {
        SwitchTarget(target.transform);
    }
    public void GetNewTarget (Vector3 pos, int damage, IDamageable damageable)
    {
        if(canSwitchTarget == true)
        {
            StartCoroutine(TickTimer());

            GameObject temp = new GameObject();
            temp.transform.position = pos;
            cam.Follow = temp.transform;
            FrameCenter();
            Destroy(temp, 2f);
        }
    }

    void FrameLower(Transform target) //Position target at the bottom of screen
    {
        transposer.m_ScreenY = 0.65f;
    }

    void FrameCenter(GameObject target, Vector3 pos, Quaternion rotation) //Position target at the center of screen
    {
        transposer.m_ScreenY = 0.5f;
    }

    void FrameCenter()
    {
        transposer.m_ScreenY = 0.5f;
    }

    IEnumerator TickTimer() //Timer should be only used for events that the player doesn't control
    {
        canSwitchTarget = false;
        yield return new WaitForSeconds(switchTimerSeconds);
        canSwitchTarget = true;
    }
}
