using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [Header("Listening To")]

    [Header("Broadcasting To")]
    public ProjectileEventsChannelSO projectileEvents;

    [Header("Projectile Settings")]
    Rigidbody2D rb = null;
    [SerializeField] float impulse = 1000f;
    public float baseDamage = 500f;
    public float forgiveness = 50f;

    [Header("Projectile Trail")]
    [SerializeField] GameObject trailPrefab;
    
    public ILaunch projLaunch;
    public ITrajectory projTrajectory;
    public IHit projHit;

    [Header("Explosion")]
    public GameObject explosion;

    private void OnEnable() 
    {
        rb = GetComponent<Rigidbody2D>();
        projLaunch = GetComponent<ILaunch>();
        projTrajectory = GetComponent<ITrajectory>();
        projHit = GetComponent<IHit>();
        
        //Broadcast Spawn
        projectileEvents.SpawnEvent.OnEventRaised(this.gameObject);
    }

    private void Start() {
        
    }

    public void ProjLaunch(float power) 
    {
        projLaunch.Launch(impulse, power);
    }
}
