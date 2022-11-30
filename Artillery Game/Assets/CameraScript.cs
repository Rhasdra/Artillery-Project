using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    GameObject currentChar = null;

    private void Awake() 
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable() 
    {
        TurnsManager.StartTurn.AddListener(GetCurrentChar);
    }

    void GetCurrentChar()
    {
        RemoveOldChar();
        currentChar = TurnsManager.currentChar.gameObject;

        GetNewTarget(currentChar.transform);
    }

    void RemoveOldChar()
    {
        
    }

    public void GetNewTarget (Transform target)
    {
        cam.Follow = target.transform;
    }

    void FindProjectile()
    {
        if(ObjectLists.projectiles.Count != 0)
        GetNewTarget(ObjectLists.projectiles[0].transform);
    }
}
