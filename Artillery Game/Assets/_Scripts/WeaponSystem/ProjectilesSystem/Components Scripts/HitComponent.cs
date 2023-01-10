using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitComponent : ProjectileComponent, IHit
{
    protected override void OnEnable()
    {
        //Add to components List
        manager.hitComponents.Add(this);

        //Listen to Launch Event
        // manager.OnLaunch += Launch;
        manager.OnProjectileHit += Hit;
    }
    
    protected override void OnDisable() 
    {
        // Remove From List
        manager.hitComponents.Remove(this);

        //Stop Listening to Launch Event
        // manager.OnLaunch -= Launch;
        manager.OnProjectileHit -= Hit;
    }

    public abstract void Hit(Collider2D col);

    public void Hit(Collider2D col, float damage)
    {
    }
}
