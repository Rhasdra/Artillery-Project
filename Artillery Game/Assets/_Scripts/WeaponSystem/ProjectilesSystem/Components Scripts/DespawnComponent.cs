using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DespawnComponent : ProjectileComponent, IProjectileDespawn
{
    protected override void OnEnable() 
    {
        //Add to components List
        manager.despawnComponents.Add(this);

        //Listen to Launch Event
        manager.OnDespawnProjectile += OnDespawn;
    }
    
    protected override void OnDisable() 
    {
        // Remove From List
        manager.despawnComponents.Remove(this);

        //Stop Listening to Launch Event
        manager.OnDespawnProjectile -= OnDespawn;
    }

    public abstract void OnDespawn();
}
