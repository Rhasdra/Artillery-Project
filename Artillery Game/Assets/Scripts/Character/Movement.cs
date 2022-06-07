using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharManager))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class Movement : MonoBehaviour
{
    CharManager charManager = null;
    CharSO charInfo = null;
    Rigidbody2D rb = null;
    CapsuleCollider2D col = null;
    Vector3 lastPos;

    [SerializeField] float horizontalInput = 0f;
    [SerializeField] bool canMove = false;

    [Header("Debug")]
    [SerializeField] bool debug = false;

    private void Awake() 
    {
        charManager = this.GetComponent<CharManager>();
        charInfo = charManager.charInfo;

        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<CapsuleCollider2D>();
 
    }
    
    void OnEnable()
    {
        charManager?.OnLongJumpPress.AddListener(LongJump);
        charManager?.OnBackFlipPress.AddListener(BackflipJump);
        charManager?.MovementInputValue.AddListener(GetInputValue);
        charManager?.EndTurn.AddListener(EndTurn);
        charManager?.StartTurn.AddListener(StartTurn);
    }

    void OnDisable()
    {
        charManager?.OnLongJumpPress.RemoveListener(LongJump);
        charManager?.OnBackFlipPress.RemoveListener(BackflipJump);
        charManager?.MovementInputValue.RemoveListener(GetInputValue);
        charManager?.EndTurn.RemoveListener(EndTurn);
        charManager?.StartTurn.RemoveListener(StartTurn);
    }

    private void FixedUpdate() 
    {        
        CharacterTilt(GetDesiredUpOutter());

        if (charManager.isMyTurn == false)
            return;

        GetCanMove();
        MoveHorizontally(horizontalInput);
    }



    public void GetInputValue(Vector2 inputValue)
    {
        horizontalInput = inputValue.x;
    }

    public void MoveHorizontally(float inputValue)
    {
        if (canMove == false)
        {
            return;
        }

        //Flip player if going left
        if (inputValue != 0)
        {
            CharacterFlip(Mathf.RoundToInt(inputValue));
        }

        transform.Translate (Vector3.right * (inputValue * charInfo.movementSpeed * Time.deltaTime));
    }    

    public void CharacterFlip(float inputValue) 
    {
            transform.localScale = new Vector3 (inputValue, 1f, 1f);
    }

    public Vector3 GetDesiredUpOutter() 
    {   
        if (transform.position == lastPos || canMove == false)
        {
            return transform.up;
        }
        
        Vector3 _rayLeftPosition = new Vector3 (transform.position.x - ((col.size.x/2) * 0.95f), transform.position.y , transform.position.z);
        Vector3 _rayRightPosition = new Vector3 (transform.position.x + ((col.size.x/2) * 0.95f), transform.position.y , transform.position.z);
        
        //Cast Raycasts
        RaycastHit2D _rayLeft = Physics2D.Raycast ( _rayLeftPosition , -Vector3.up , 5f, LayerMask.GetMask("Terrain"));
        RaycastHit2D _rayRight = Physics2D.Raycast (_rayRightPosition , -Vector3.up, 5f, LayerMask.GetMask("Terrain"));

        if (debug)
        {
            Debug.Log("CharacterTilt was called");
        }

        //Lerp extremities
        Vector2 lerp = Vector2.Lerp (_rayLeft.normal , _rayRight.normal , 0.5f);

        //Lerp extremities with middle raycast
        return Vector2.Lerp (lerp , GetDesiredUpInner(), 0.75f);
    }

    public Vector3 GetDesiredUpInner() 
    {   
        if (transform.position == lastPos || canMove == false)
        {
            return transform.up;
        }
        
        Vector3 _rayCenter = new Vector3 (transform.position.x, transform.position.y , transform.position.z);
        Vector3 _rayTop = new Vector3 (transform.position.x, transform.position.y + (col.size.y/2), transform.position.z);
        //Cast Raycasts
        RaycastHit2D _raySelf = Physics2D.Raycast ( _rayCenter , -transform.up , (col.size.y/2)*1.2f, LayerMask.GetMask("Terrain"));
        RaycastHit2D _rayWorld = Physics2D.Raycast (_rayTop , -Vector3.up, (col.size.y*1.2f), LayerMask.GetMask("Terrain"));

        //Lerp slowly
        return Vector2.Lerp (_rayWorld.normal , _raySelf.normal , 0.75f);
    }

    void CharacterTilt(Vector3 desiredUp)
    {
        float acc = 10f;
        transform.up += (desiredUp - transform.up) * Time.deltaTime * acc;
    }

    public void LongJump()
    {
        if ( canMove == false )
            { return; }

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

    public void GetCanMove()
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
            isGrounded = true;
            break;
            }
        }

        // if( Mathf.Abs(rb.velocity.y) < 1.5f)
        // {
        //     isJumping = false;
        // }
        // else
        // {
        //     isJumping = true;
        // }


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
    
    public void StartTurn()
    {

    }

    public void EndTurn()
    {

    }

}
