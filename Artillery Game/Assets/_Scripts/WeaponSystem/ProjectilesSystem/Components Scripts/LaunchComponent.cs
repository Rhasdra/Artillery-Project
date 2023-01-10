using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LaunchComponent : ProjectileComponent, ILaunch
{
    protected override void OnEnable() 
    {
        //Add to components List
        manager.launchComponents.Add(this);

        //Listen to Launch Event
        // manager.OnLaunch += Launch;
        manager.OnLaunch += Launch;
    }
    
    protected override void OnDisable() 
    {
        // Remove From List
        manager.launchComponents.Remove(this);

        //Stop Listening to Launch Event
        // manager.OnLaunch -= Launch;
        manager.OnLaunch -= Launch;
    }

    public abstract void Launch(float impulse, float power);
}
