using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileComponent : MonoBehaviour
{
    [Header("Settings")]
    public int priority = 50;

    protected ProjectileManager manager = null;
    protected Rigidbody2D rb = null;

    private void Awake() 
    {
        if(manager == null)
            manager = GetComponent<ProjectileManager>();

        if(rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    protected abstract void OnEnable();
    protected abstract void OnDisable();
}
