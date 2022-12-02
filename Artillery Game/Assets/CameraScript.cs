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


    [SerializeField] GameObject tempGO;
    public CinemachineVirtualCamera cam;
    CinemachineFramingTransposer transposer;
    GameObject currentChar = null;

    private void Awake() 
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void OnEnable() 
    {
        //TurnsManager.StartTurn.AddListener(GetCurrentChar);

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

    // void GetCurrentChar()
    // {
    //     RemoveOldChar();
    //     currentChar = TurnsManager.currentChar.gameObject;

    //     GetNewTarget(currentChar.transform);
    // }

    // void RemoveOldChar()
    // {
        
    // }

    public void GetNewTarget (Transform target)
    {
        cam.Follow = target.transform;
    }
    public void GetNewTarget (GameObject target)
    {
        cam.Follow = target.transform;
    }
    public void GetNewTarget (GameObject target, Vector3 pos, Quaternion rotation)
    {
        cam.Follow = target.transform;
    }
    public void GetNewTarget (Vector3 pos, int damage, IDamageable damageable)
    {
        GameObject temp = Instantiate(tempGO, pos, Quaternion.identity);
        cam.Follow = temp.transform;
        FrameCenter();
        Destroy(temp, 3f);
    }

    void FrameLower(Transform target)
    {
        transposer.m_ScreenY = 0.65f;
    }

    void FrameCenter(GameObject target, Vector3 pos, Quaternion rotation)
    {
        transposer.m_ScreenY = 0.5f;
    }

    void FrameCenter()
    {
        transposer.m_ScreenY = 0.5f;
    }
}
