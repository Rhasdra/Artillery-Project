using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructOnLoad : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject);   
    }
}
