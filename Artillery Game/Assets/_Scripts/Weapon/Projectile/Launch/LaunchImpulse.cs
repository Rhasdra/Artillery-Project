using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchImpulse : MonoBehaviour, ILaunch
{
    public Rigidbody2D rb;

    private void OnEnable() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public virtual void Launch(float impulse, float power)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.RoundToInt(transform.rotation.eulerAngles.z));
        rb.AddForce(transform.right * impulse * (power/100f));

        //Broadcast the Event
        GetComponent<Projectile>().projectileEvents.LaunchEvent.OnEventRaised(transform.position);
    }
}
