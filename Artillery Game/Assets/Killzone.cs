using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    Collider2D[] colliders;

    private void Awake() 
    {
        colliders = GetComponents<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        //Debug.Log("other");
        other.gameObject.GetComponentInParent<CharManager>().Die();
    }
}
