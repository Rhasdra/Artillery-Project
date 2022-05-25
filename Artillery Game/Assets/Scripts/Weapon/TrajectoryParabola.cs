using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryParabola : MonoBehaviour , ITrajectory
{
    Rigidbody2D rb;

    private void OnEnable() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate() 
    {
        UpdateTrajectory();
    }
        
    public void UpdateTrajectory()
    {
        float projectileAngle;

        //Glitches if updates on the first frame where the velocity is 0??
        if (rb.velocity.y != 0f)
        {
        projectileAngle = Mathf.Atan2(rb.velocity.y , rb.velocity.x) * Mathf.Rad2Deg;

        transform.localRotation = Quaternion.AngleAxis(projectileAngle,transform.forward);
        }
    }

}
