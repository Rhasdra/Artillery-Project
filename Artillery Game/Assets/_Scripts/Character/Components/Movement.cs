using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharManager))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class Movement : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] InputReader inputReader;

    [Header("Broadcasting To")]
    [SerializeField] MovementEventsChannelSO movementEvents;

    CharSO charInfo = null;
    Rigidbody2D rb = null;
    CapsuleCollider2D col = null;
    Vector3 lastPos;
    Vector3 lastTiltPos;

    [Header("References")]
    [SerializeField] Transform raycastsPos;

    [Header("Information and Configs")]
    [SerializeField] float horizontalInput = 0f;
    [SerializeField] bool canMove = false;
    [SerializeField] float tiltSpeed = 2f;
    [SerializeField] float tiltThresholdAngle = 3f;
    [SerializeField] float floorAngle;

    [Header("Debug")]
    [SerializeField] bool debug = false;

    private void Awake() 
    {
        charInfo = this.GetComponent<CharManager>().charInfo;

        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<CapsuleCollider2D>();
    }
    
    void OnEnable()
    {
        inputReader.LongJumpEvent += LongJump;
        inputReader.BackFlipEvent += BackflipJump;
        inputReader.MoveEvent += GetInputValue;
    }

    void OnDisable()
    {
        inputReader.LongJumpEvent -= LongJump;
        inputReader.BackFlipEvent -= BackflipJump;
        inputReader.MoveEvent -= GetInputValue;
    }

    private void LateUpdate() 
    {        
        CharacterTilt(Raycasts());
    }

    private void FixedUpdate()
    { 
        GetCanMove();
        MoveHorizontally(horizontalInput);
    }

    public void GetInputValue(Vector2 inputValue)
    {
        horizontalInput = inputValue.x;
    }

    public void MoveHorizontally(float inputValue)
    {
        if (canMove == false || inputValue == 0)
        {
            return;
        }

        movementEvents.MoveStartEvent.RaiseEvent(this.transform);

        //Flip player if going left
        if (inputValue != 0)
        {
            CharacterFlip(Mathf.RoundToInt(inputValue));
        }

        transform.Translate (Vector3.right * (inputValue * charInfo.movementSpeed * Time.deltaTime) * ClimbSlowMultiplier());
    }    
    
    float ClimbSlowMultiplier()
    {            
        // Calculate floor angle
        floorAngle = Vector3.Angle(transform.right, -Vector3.up);
        if (transform.localScale.x < 0)
        {
            floorAngle = Vector3.Angle(transform.right, Vector3.up);
        }
        floorAngle -= 90;

        if ( floorAngle <= charInfo.climbAngle * .4)
        {
            return 1f;
        }
        else if ( floorAngle >= charInfo.climbAngle)
        {
            return 0f;
        }
        else
        {
            return Mathf.Lerp(1, 0, floorAngle / charInfo.climbAngle);
        }
    }

    void CharacterFlip(float inputValue) 
    {
            transform.localScale = new Vector3 (inputValue, 1f, 1f);
    }
    
    Vector3 Raycasts() 
    {   
        if (transform.position == lastPos || canMove == false)
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
            RaycastHit2D ray = Physics2D.Raycast ( rayPos[i] , -transform.up , 5f, LayerMask.GetMask("Terrain"));
            hitNormals[i] = ray.normal;
            Debug.DrawRay(rayPos[i] , -transform.up);
        }

        Vector2 average = new Vector2();

        for (int i = 0; i < hitNormals.Length; i++)
        {
            average += hitNormals[i];
        }

        RaycastHit2D backupOnTop = Physics2D.Raycast (new Vector3( transform.position.x, transform.position.y + (col.size.y/2), transform.position.z) , -Vector3.up , 5f, LayerMask.GetMask("Terrain"));

        return average + backupOnTop.normal;
    }

    void CharacterTilt(Vector3 desiredUp)
    {        
        float angle = Vector3.Angle(desiredUp, transform.up);
        
        if(angle >= tiltThresholdAngle)
        {
            transform.up += (desiredUp - transform.up) * Time.deltaTime * tiltSpeed;
            lastTiltPos = transform.position;
        }
        else
        {
            return;
        }
    }

    public void LongJump()
    {
        if ( canMove == false )
            { return; }
            
        //Broadcast the event
        movementEvents.LongJumpEvent.RaiseEvent(this.transform);

        rb.velocity = new Vector2 (transform.localScale.x * charInfo.longJumpForce, charInfo.longJumpForce);

        if (debug)
        {
            Debug.Log("LongJump() Performed");
        }
    }

    public void BackflipJump()
    {    
        if ( canMove == false )
            { return; }

        //Broadcast the event
        movementEvents.BackFlipJumpEvent.RaiseEvent(this.transform);
            
        rb.velocity = new Vector2 (transform.localScale.x * charInfo.backFlipJumpForceX, charInfo.backFlipJumpForceY);
        StartCoroutine("BackFlip");
    }

    IEnumerator BackFlip()
    {
        float startRotation = transform.rotation.eulerAngles.z;
    
        for (int i = 0; i <= 60f; i++)
        {
            transform.rotation = Quaternion.Euler ( 0f, 0f, startRotation + (i * 6f * transform.localScale.x));

            if(rb.velocity.y < 0 && canMove)
            {
                transform.rotation = Quaternion.identity;
                StopCoroutine("BackFlip");
            }

            yield return new WaitForSeconds(1f/60f);
        }
    }

    void GetCanMove()
    {
        bool isGrounded = false;
        bool isJumping = false;
        
        float _rayHeight = (col.size.y/2) * 1.4f;
        float _rayRadius = (col.size.x/2) * 1.2f;
        float rays = 10f;

        // cast down raycasts along the radius until one of them returns true, then isGrounded == true
        for (int i = 0; i < (rays + 1); i++)
        {
            float x = Mathf.Lerp(transform.position.x - _rayRadius, transform.position.x + _rayRadius, i / rays);
            Vector3 position = new Vector3(x, transform.position.y, transform.position.z);
            bool hit = Physics2D.Raycast (position, -transform.up, _rayHeight, LayerMask.GetMask("Terrain"));

            if (debug)
            {Debug.DrawRay(position, -transform.up, Color.red);
            Debug.Log("x: " + x + " hit " + hit);}
            
            if(hit == true)
            {
                //Broadcast the event
                movementEvents.LandingEvent.RaiseEvent(transform.position);
                
                isGrounded = true;
                break;
            }
        }

        if(isGrounded==true && isJumping==false)
        {
            canMove = true;
        }else
        {
            canMove = false;
        }

        Vector3 newPos = transform.position;
        if(Mathf.Approximately(lastPos.y , newPos.y))
        {
            canMove = true;
        }

        lastPos = newPos;
    }
}
