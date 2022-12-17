using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsUp : CheckForGround
{  
    [SerializeField] Transform raycastsPos;
    [SerializeField] Rigidbody2D rb;

    // [SerializeField] bool isGrounded = false;

    [Header("Info")]
    [SerializeField] bool isStunned = false;

    [Header("Configs")]
        [Tooltip("Character will only rotate if the ground has an angle difference of X or greater.")]
    [SerializeField] float tiltThresholdAngle = 3f;
        [Tooltip("How fast the character will rotate to match the ground. Smaller = slower.")]
    [SerializeField] float tiltSpeed = 10f;
    [SerializeField] float forceTiltSpeed = 180f;
    [SerializeField] float rayLength = 1f;

    protected override void Awake() 
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate() 
    {        
        CharacterTilt(Raycasts());
    }

    Vector3 Raycasts() 
    {   
        if (transform.position == lastPos)
        {
            return transform.up;
        }
        
        Vector3[] rayPos = {
        new Vector3 (raycastsPos.position.x - ((col.size.x/2) * 0.95f), raycastsPos.position.y, transform.position.z),
        new Vector3 (raycastsPos.position.x + ((col.size.x/2) * 0.95f), raycastsPos.position.y , transform.position.z),
        new Vector3 (raycastsPos.position.x, raycastsPos.position.y , transform.position.z)
        };
        
        //Cast Raycasts
        Vector2[] hitNormals = new Vector2[3];
        
        for (int i = 0; i < rayPos.Length; i++)
        {
            RaycastHit2D ray = Physics2D.Raycast ( rayPos[i] , -transform.up , rayLength, LayerMask.GetMask("Terrain"));
            hitNormals[i] = ray.normal;
            Debug.DrawRay(rayPos[i] , -transform.up);
        }

        Vector2 average = new Vector2();
        for (int i = 0; i < hitNormals.Length; i++)
        {
            average += hitNormals[i];
        }

        RaycastHit2D backupOnTop = Physics2D.Raycast (new Vector3( transform.position.x, transform.position.y + (col.size.y/2), transform.position.z) , -Vector3.up , rayLength, LayerMask.GetMask("Terrain"));
        return average + backupOnTop.normal;
    }

    void CharacterTilt(Vector3 desiredUp)
    {   
        if(IsGrounded() == false || isStunned == true)
            return;

        // Check if character is sideways
        if(transform.localEulerAngles.z > 40 && transform.localEulerAngles.z < 180)
        {
            // Debug.Log(transform.localEulerAngles.z);
            transform.Rotate(0, 0, -(forceTiltSpeed * Time.deltaTime));
            return;
        }
        else if(transform.localEulerAngles.z > 180 && transform.localEulerAngles.z < 320)
        {
            // Debug.Log(transform.localEulerAngles.z);
            transform.Rotate(0, 0, (forceTiltSpeed * Time.deltaTime));
            return;
        }

        // If not sideways, match ground
        float angle = Vector3.Angle(desiredUp, transform.up);
        if(angle >= tiltThresholdAngle)
        {
            transform.up += (desiredUp - transform.up) * Time.deltaTime * tiltSpeed;
            lastPos = transform.position;
        }
    }

    public IEnumerator Stun(float stunSeconds)
    {
        isStunned = true;
        while(isStunned == true)
        {
            stunSeconds -= Time.deltaTime;
            if(stunSeconds < 0)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                isStunned = false;
            }
            else
            {
                yield return null;
            }

        }
    }
}
