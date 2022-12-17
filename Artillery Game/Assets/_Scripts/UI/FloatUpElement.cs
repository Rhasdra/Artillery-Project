using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpElement : MonoBehaviour
{
    public float TimerSeconds = 1f;
    [HideInInspector] protected float Timer = 1f;

    [SerializeField] protected float speed = 1f;

    private void OnEnable() 
    {
        Timer = TimerSeconds;
    }

    protected virtual void Update() 
    {
        TickTimer();
        GoUp();
    }

    protected virtual void TickTimer()
    {
        Timer -= Time.deltaTime;
        //Debug.Log(timer);

        if(Timer < 0)
        {
            Destroy(this.gameObject, Timer);
        }
    }

    protected void GoUp()
    {
        transform.position = new Vector3 (transform.position.x, transform.position.y + (0.2f * speed * Time.deltaTime), transform.position.z);
    }
}
