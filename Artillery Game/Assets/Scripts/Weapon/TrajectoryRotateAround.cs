using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRotateAround : MonoBehaviour, ITrajectory
{
    public Transform pivotProj;
    public float degreesPerSec = -180;

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
        if(pivotProj != null)
        transform.RotateAround(pivotProj.position, Vector3.forward, degreesPerSec * Time.deltaTime * Mathf.Sign(rb.velocity.x));
    }
}
