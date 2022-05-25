using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchImpulse : MonoBehaviour, ILaunch
{
    Rigidbody2D rb;

    private void OnEnable() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void Launch(float impulse, float power)
    {
        rb.AddForce(transform.right * impulse * (power/100f));
    }
}
