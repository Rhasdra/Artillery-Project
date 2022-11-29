using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

    private void Awake() 
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void GetNewTarget (Transform target)
    {
        cam.Follow = target;
    }
}
