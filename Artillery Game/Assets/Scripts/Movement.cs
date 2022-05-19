using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
    CharSO charInfo = null;
    Rigidbody2D rb = null;
    CircleCollider2D col = null;

    [SerializeField]bool canMove = false;

    [Header("Debug")]
    [SerializeField] CircleCollider2D debugCol = null;
    [SerializeField] bool debug = false;

    private void Awake() 
    {
        charInfo = this.GetComponent<CharInfo>().charInfo;
        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<CircleCollider2D>();
    }
    
    private void FixedUpdate() 
    {
        if (new Vector2(rb.velocity.y , rb.velocity.x) != Vector2.zero)
        {
            CharacterTilt();
        }
    }

    public void MoveHorizontally(float inputValue)
    {
        //Flip player if going left
        if (inputValue != 0)
        {
            CharacterFlip(inputValue);
        }

        transform.Translate (Vector3.right * (inputValue * charInfo.movementSpeed * Time.deltaTime));
    }    

    public void CharacterFlip(float inputValue) 
    {
            transform.localScale = new Vector3 (inputValue, 1f, 1f);
    }

    public void CharacterTilt() 
    {   
        Vector3 _rayLeftPosition = new Vector3 (transform.position.x - col.radius, transform.position.y , transform.position.z);
        Vector3 _rayRightPosition = new Vector3 (transform.position.x + col.radius, transform.position.y , transform.position.z);
        
        //Cast Raycasts
        RaycastHit2D _rayLeft = Physics2D.Raycast ( _rayLeftPosition , -Vector3.up , 5f, LayerMask.GetMask("Terrain"));
        RaycastHit2D _rayRight = Physics2D.Raycast (_rayRightPosition , -Vector3.up, 5f, LayerMask.GetMask("Terrain"));

        //Rotate player according to terrain
        transform.up = Vector2.Lerp (_rayLeft.normal , _rayRight.normal , 0.5f);

        if (rb.velocity.y == 0f)
        {
            canMove = true;
        }

        if (debug)
        {
            Debug.Log("CharacterTilt was called");
        }
    }

        public void LongJump()
    {
        canMove = false;
        rb.velocity = new Vector2 (transform.localScale.x * charInfo.longJumpForce, charInfo.longJumpForce);
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

        if(canMove)
        {
            StopCoroutine("BackFlip");
        }

        yield return new WaitForSeconds(1f/60f);
    }
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
