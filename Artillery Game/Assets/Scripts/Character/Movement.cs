using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharManager))]
[RequireComponent(typeof(CircleCollider2D))]

public class Movement : MonoBehaviour
{
    CharManager charManager = null;
    CharSO charInfo = null;
    Rigidbody2D rb = null;
    CircleCollider2D col = null;
    Vector3 lastPos;

    [SerializeField] float horizontalInput = 0f;
    [SerializeField] bool canMove = false;

    [Header("Debug")]
    [SerializeField] CircleCollider2D debugCol = null;
    [SerializeField] bool debug = false;

    private void Awake() 
    {
        charManager = this.GetComponent<CharManager>();
        charInfo = charManager.charInfo;

        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<CircleCollider2D>();
 
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
        CharacterTilt(GetDesiredUpLerp());

        if (charManager.isMyTurn == false)
            return;

        GetCanMove();
        MoveHorizontally(horizontalInput);

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

    public Vector3 GetDesiredUpLerp() 
    {   
        if (transform.position == lastPos || canMove == false)
        {
            return transform.up;
        }

        Vector3 _rayLeftPosition = new Vector3 (transform.position.x - (col.radius * 0.95f), transform.position.y , transform.position.z);
        Vector3 _rayRightPosition = new Vector3 (transform.position.x + (col.radius * 0.95f), transform.position.y , transform.position.z);
        
        //Cast Raycasts
        RaycastHit2D _rayLeft = Physics2D.Raycast ( _rayLeftPosition , -Vector3.up , 5f, LayerMask.GetMask("Terrain"));
        RaycastHit2D _rayRight = Physics2D.Raycast (_rayRightPosition , -Vector3.up, 5f, LayerMask.GetMask("Terrain"));

        if (debug)
        {
            Debug.Log("CharacterTilt was called");
        }

        //Rotate player according to terrain
        Vector2 lerp = Vector2.Lerp (_rayLeft.normal , _rayRight.normal , 0.5f);

        return Vector2.Lerp (lerp , GetDesiredUp(), 0.5f);
    }

    public Vector3 GetDesiredUp() 
    {   
        if (transform.position == lastPos || canMove == false)
        {
            return transform.up;
        }

        Vector3 _rayPosition = new Vector3 (transform.position.x , transform.position.y , transform.position.z);
        
        //Cast Raycasts
        RaycastHit2D _ray = Physics2D.Raycast ( _rayPosition , -Vector3.up , 5f, LayerMask.GetMask("Terrain"));

        if (debug)
        {
            Debug.Log("CharacterTilt was called");
        }

        //Rotate player according to terrain
        return _ray.normal;
    }

    void CharacterTilt(Vector3 desiredUp)
    {
        float acc = 20f;
        transform.up += (desiredUp - transform.up) * Time.deltaTime * acc;
    }

    public void LongJump()
    {
        rb.velocity = new Vector2 (transform.localScale.x * charInfo.longJumpForce, charInfo.longJumpForce);

        if (debug)
        {
            Debug.Log("LongJump() Performed");
        }
    }

    public void BackflipJump()
    {    
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

    }

    public void EndTurn()
    {

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
