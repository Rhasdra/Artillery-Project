using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUntilHitGround : CheckForGround
{   
    Rigidbody2D rb;
    float timer = 2f;

    protected override void Awake() 
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        if(IsGrounded() == false)
            Rotate();
        else
        {
            TickTime();
        }
    }

    void Rotate()
    {
        Debug.Log("Rotate");
        if(rb.velocity.x > 0f)
            transform.Rotate(0, 0, -(360 * Time.deltaTime * 2), Space.Self);
        else if(rb.velocity.x < 0f)
            transform.Rotate(0, 0, (360 * Time.deltaTime * 2), Space.Self);
    }

    void TickTime()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            Destroy(this);
        }
    }
}
