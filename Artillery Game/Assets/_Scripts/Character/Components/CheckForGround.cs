using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckForGround : MonoBehaviour
{
    [Header("Broadcasting to:")]
    [SerializeField] CharRaycastEventsChannelSO rayEvents;

    [SerializeField] protected Vector3 lastPos;
    float lastPosTime;
    [SerializeField] protected CapsuleCollider2D col;
    [SerializeField] bool isOn = false;

    float stationarySeconds = 0.5f;

    public UnityEvent<bool> IsGroundedEvent;

    protected virtual void Awake() 
    {
        col = GetComponent<CapsuleCollider2D>();
    }

    private void OnDisable() 
    {
        isOn = false;
    }

    protected bool IsGrounded()
    {    
        float _rayHeight = (col.size.y/2) * 1.2f;
        float _rayRadius = (col.size.x/2) * 1.2f;
        float rays = 10f;

        if(lastPos == transform.position) // If stationary you must probably grounded
        {            
            if((Time.time - lastPosTime) > stationarySeconds) //Check if not stationary briefly in the top of a jump arch
            {
                isOn = true;
                //Broadcast success
                // Debug.Log("IsGrounded time");
                IsGroundedEvent.Invoke(true);
                //rayEvents.IsGroundedEvent.RaiseEvent(true);
                return true;
            }                
        }

        // cast down raycasts along the radius until one of them returns true, then isGrounded == true
        for (int i = 0; i < (rays + 1); i++)
        {
            float x = Mathf.Lerp(col.bounds.center.x - _rayRadius, col.bounds.center.x + _rayRadius, i / rays);
            Vector3 position = new Vector3(x, col.bounds.center.y, transform.position.z);
            bool hit = Physics2D.Raycast (position, -Vector2.up, _rayHeight, LayerMask.GetMask("Terrain"));

            Debug.DrawRay(position, -Vector2.up, Color.red);
            
            if(hit == true)
            { 
                isOn = true;
                //Broadcast success
                // Debug.Log("IsGrounded Raycast");
                IsGroundedEvent.Invoke(true);
                //rayEvents.IsGroundedEvent.RaiseEvent(true);
                return true;
            }
        }

        lastPos = transform.position;
        if(isOn)
        {
            lastPosTime = Time.time;
            isOn = false;
        }
        //Broadcast failure
        IsGroundedEvent.Invoke(false);
        //rayEvents.IsGroundedEvent.RaiseEvent(false);
        return false;
    }
}
