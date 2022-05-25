using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform followPoint;
    public float offset = 1f;

    void LateUpdate () 
    {
        if (followPoint != null && followPoint.gameObject.activeSelf)
        {
        this.gameObject.SetActive(true);
        transform.position = new Vector3(followPoint.position.x , (followPoint.position.y - offset) , followPoint.position.z);
        }
        else if (followPoint.gameObject.activeSelf == false)
        this.gameObject.SetActive(false);
    }
}
