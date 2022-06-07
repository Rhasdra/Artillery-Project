using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform followPoint;
    public float offset = 0f;

    [SerializeField] bool rotate = false;
    [SerializeField] bool flippable = false;

    void LateUpdate () 
    {
        if (followPoint != null && followPoint.gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.position = new Vector3(followPoint.position.x , (followPoint.position.y - offset) , followPoint.position.z);
            
            if(rotate)
                transform.rotation = followPoint.rotation;

            if(flippable)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * followPoint.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (followPoint.gameObject.activeSelf == false)
            transform.GetChild(0).gameObject.SetActive(false);
    }
}
