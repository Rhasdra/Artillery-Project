using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnComponent : ProjectileComponent, IProjectileSpawn
{
    protected override void OnEnable() 
    {
        //Add to components List
        manager.spawnComponents.Add(this);

        //Listen to Launch Event
        // manager.OnLaunch += Launch;
        manager.OnSpawnProjectile += OnSpawn;
    }
    
    protected override void OnDisable() 
    {
        // Remove From List
        manager.spawnComponents.Remove(this);

        //Stop Listening to Launch Event
        // manager.OnLaunch -= Launch;
        manager.OnSpawnProjectile -= OnSpawn;
    }

    public abstract void OnSpawn();
}
