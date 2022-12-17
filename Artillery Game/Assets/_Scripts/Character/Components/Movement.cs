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
    [SerializeField] CharRaycastEventsChannelSO rayEvents;

    [Header("Broadcasting To")]
    [SerializeField] MovementEventsChannelSO movementEvents;

    [Header("References")]
        [Tooltip("A character should have a empty GO at their feet, from where the raycasts will originate and rotate around.")]
    CharSO charInfo = null;
    Rigidbody2D rb = null;
    CapsuleCollider2D col = null;
    Vector3 lastPos;
    Vector3 lastTiltPos;

    [Header("Info")]
    float horizontalInput = 0f;
    [SerializeField] bool canMove = false;
    float floorAngle;
    public bool isGrounded = false;

    [Header("Delay")]
    public int moveDelay = 2;
    [SerializeField] float distThreshold = 0.5f;
    bool hasMoved = false;
    Vector3 startingPos;

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

        rayEvents.IsGroundedEvent.OnEventRaised += GetCanMove;

        startingPos = transform.position;
        hasMoved = false;
    }

    void OnDisable()
    {
        inputReader.LongJumpEvent -= LongJump;
        inputReader.BackFlipEvent -= BackflipJump;
        inputReader.MoveEvent -= GetInputValue;

        rayEvents.IsGroundedEvent.OnEventRaised -= GetCanMove;

        //Reseting values for when ending turn with button held
        horizontalInput = 0f;
    }

    // private void LateUpdate() 
    // {        
    //     CharacterTilt(Raycasts());
    // }

    private void FixedUpdate()
    { 
        MoveHorizontally(horizontalInput);
    }

    public void GetInputValue(float inputValue)
    {
        horizontalInput = inputValue;
    }

    public void MoveHorizontally(float inputValue)
    {
        if (canMove == false || inputValue == 0) //Check if can move
        {
            return;
        }

        //Flip player if going left
        if (inputValue != 0)
        {
            CharacterFlip(Mathf.RoundToInt(inputValue));
        }

        transform.Translate (Vector3.right * (inputValue * charInfo.movementSpeed * Time.deltaTime) * ClimbSlowMultiplier());
    
        //Raise moveStart event
        movementEvents.MoveStartEvent.RaiseEvent(this.transform);

        //Delay check
        ThresholdCheck();
    }

    void ThresholdCheck() //Check if character crossed the amount it can move before triggering the delay addition
    {
        if (hasMoved == true)
        return;

        float distWalked = Mathf.Abs(startingPos.x - transform.position.x);

        if(distWalked > distThreshold)
        {
            movementEvents.ThresholdCrossedEvent.OnEventRaised();
            hasMoved = true;
        }        
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

    public void GetCanMove(bool b)
    {
        canMove = b;
    }
}
