using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public GameObject explosion;

    Rigidbody2D rb = null;
    [SerializeField] float impulse = 1000f;
    public float baseDamage = 500f;
    public float forgiveness = 50f;
    
    public ILaunch projLaunch;
    public ITrajectory projTrajectory;
    public IHit projHit;

    public UnityEvent<Vector3, float> ProjectileHit;

    private void OnEnable() 
    {
        rb = GetComponent<Rigidbody2D>();
        projLaunch = GetComponent<ILaunch>();
        projTrajectory = GetComponent<ITrajectory>();
        projHit = GetComponent<IHit>();
    }

    public void ProjLaunch(float power) 
    {
        projLaunch.Launch(impulse, power);
    }
}
