using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch_Impulse : LaunchComponent
{
    public override void Launch(float impulse, float power)
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.RoundToInt(transform.rotation.eulerAngles.z));
        rb.AddForce(transform.right * impulse * (power/100f));
    }
}
