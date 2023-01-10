using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrajectoryComponent : ProjectileComponent, ITrajectory
{
    protected override void OnEnable()
    {
        //Add to components List
        manager.trajectoryComponents.Add(this);

        //Listen to Launch Event
        // manager.OnLaunch += Launch;
        manager.OnTrajectoryTick += UpdateTrajectory;
    }
    
    protected override void OnDisable() 
    {
        // Remove From List
        manager.trajectoryComponents.Remove(this);

        //Stop Listening to Launch Event
        // manager.OnLaunch -= Launch;
        manager.OnTrajectoryTick -= UpdateTrajectory;
    }

    public abstract void UpdateTrajectory();
}
