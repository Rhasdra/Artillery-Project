using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : MonoBehaviour
{
    public int delay;

    private void OnEnable() 
    {
        GetComponent<Weapons>().ProjectileFired.AddListener(RandomizeDelay);
    }

    void RandomizeDelay()
    {
        delay = Random.Range(0,255);
    }
}
