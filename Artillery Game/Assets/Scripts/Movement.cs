using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    CharController charController = null;
    CharSO charInfo = null;
    Rigidbody2D rb = null;
    CircleCollider2D col = null;
    Vector3 lastPos;

    [SerializeField] float horizontalInput = 0f;
    [SerializeField] bool canMove = false;
    [SerializeField] bool isMyTurn = true;

    [Header("Debug")]
    [SerializeField] CircleCollider2D debugCol = null;
    [SerializeField] bool debug = false;

    private void Awake() 
    {
        charController = this.GetComponent<CharController>();
        charInfo = charController.charInfo;

        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<CircleCollider2D>();
 
    }
    
    // void OnEnable()
    // {
    //     charController?.OnLongJumpPress.AddListener(LongJump);
    //     charController?.OnBackFlipPress.AddListener(BackflipJump);
    //     charController?.OnMovementValueChanged.AddListener(GetInputValue);
    //     charController?.EndTurn.AddListener(EndTurn);
    //     charController?.StartTurn.AddListener(StartTurn);
    // }

    // void OnDisable()
    // {
    //     charController.OnLongJumpPress.RemoveListener(LongJump);
    //     charController.OnBackFlipPress.RemoveListener(BackflipJump);
    //     charController.OnMovementValueChanged.RemoveListener(GetInputValue);
    //     charController?.EndTurn.RemoveListener(EndTurn);
    //     charController?.StartTurn.RemoveListener(StartTurn);
    // }

    private void FixedUpdate() 
    {
        if (isMyTurn == false)
        {
            return;
        }

        GetCanMove();
        MoveHorizontally(horizontalInput);

        if (transform.position == lastPos)
        {
            canMove = true;
            return;
        }

        CharacterTilt();
        lastPos = transform.position;
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

    public void CharacterTilt() 
    {   
        if (canMove == false)
        {
            return;
        }

        Vector3 _rayLeftPosition = new Vector3 (transform.position.x - col.radius, transform.position.y , transform.position.z);
        Vector3 _rayRightPosition = new Vector3 (transform.position.x + col.radius, transform.position.y , transform.position.z);
        
        //Cast Raycasts
        RaycastHit2D _rayLeft = Physics2D.Raycast ( _rayLeftPosition , -Vector3.up , 5f, LayerMask.GetMask("Terrain"));
        RaycastHit2D _rayRight = Physics2D.Raycast (_rayRightPosition , -Vector3.up, 5f, LayerMask.GetMask("Terrain"));

        //Rotate player according to terrain
        transform.up = Vector2.Lerp (_rayLeft.normal , _rayRight.normal , 0.5f);

        if (debug)
        {
            Debug.Log("CharacterTilt was called");
        }
    }


    public void LongJump()
    {
        canMove = false;
        rb.velocity = new Vector2 (transform.localScale.x * charInfo.longJumpForce, charInfo.longJumpForce);

        if (debug)
        {
            Debug.Log("LongJump() Performed");
        }
    }

    public void BackflipJump()
    {    
        canMove = false;
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
            StopCoroutine("BackFlip");
        }

        yield return new WaitForSeconds(1f/60f);
    }
    }

    public void GetCanMove()
{
    bool isGrounded = false;
    bool isJumping = false;
    float _rayDistance = col.radius * 1.4f;
    Vector3 _rayLeftPosition = new Vector3 (transform.position.x - col.radius, transform.position.y , transform.position.z);
    Vector3 _rayRightPosition = new Vector3 (transform.position.x + col.radius, transform.position.y , transform.position.z);

    RaycastHit2D _rayMiddle = Physics2D.Raycast (transform.position, -Vector3.up, _rayDistance, LayerMask.GetMask("Terrain"));
    RaycastHit2D _rayLeft = Physics2D.Raycast (_rayLeftPosition , -Vector3.up , _rayDistance, LayerMask.GetMask("Terrain"));
    RaycastHit2D _rayRight = Physics2D.Raycast (_rayRightPosition , -Vector3.up, _rayDistance, LayerMask.GetMask("Terrain"));

    if (_rayMiddle || _rayLeft || _rayRight)
    {
        isGrounded = true;
    } 
    else
    {
        isGrounded = false;
    }


    if( Mathf.Abs(rb.velocity.y) < 1.5f)
    {
        isJumping = false;
    }
    else
    {
        isJumping = true;
    }


    if(isGrounded==true && isJumping==false)
    {
        canMove = true;
    }else
    {
        canMove = false;
    }
}
    public void StartTurn()
    {
        isMyTurn = true;
    }

    public void EndTurn()
    {
        isMyTurn = false;
    }



    private void OnDrawGizmos() 
    {
        if (debug)
        {
            Vector3 _rayLeftPosition = new Vector3 (transform.position.x - debugCol.radius, transform.position.y , transform.position.z);
            Vector3 _rayRightPosition = new Vector3 (transform.position.x + debugCol.radius, transform.position.y , transform.position.z);
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay ( _rayLeftPosition , -Vector3.up );
            Gizmos.DrawRay ( _rayRightPosition , -Vector3.up );
        }
    }

}
