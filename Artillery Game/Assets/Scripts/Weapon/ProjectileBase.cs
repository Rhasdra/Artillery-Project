using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] enum LaunchMode{None, Impulse, MultiShot, LaunchRotator}
    [SerializeField] LaunchMode launchMode;

    [SerializeField] enum TrajectoryMode{None, Parabola, RotateAround}
    [SerializeField] TrajectoryMode trajectoryMode;

    [SerializeField] enum HitMode{None, Missile, Grenade}
    [SerializeField] HitMode hitMode;

    public GameObject explosion;

    Rigidbody2D rb = null;
    [SerializeField] float impulse = 1000f;
    public float baseDamage = 500f;
    public float forgiveness = 50f;
    
    public ILaunch projLaunch;
    public ITrajectory projTrajectory;
    public IHit projHit;

    private void OnEnable() 
    {
        rb = GetComponent<Rigidbody2D>();

        switch (launchMode)
        {
            case LaunchMode.Impulse:
            gameObject.AddComponent<LaunchImpulse>();
            break;

            case LaunchMode.MultiShot:
            gameObject.AddComponent<LaunchMultiShot>();
            break;

            case LaunchMode.LaunchRotator:
            gameObject.AddComponent<LaunchRotator>();
            break;
        }
        projLaunch = GetComponent<ILaunch>();

        switch (trajectoryMode)
        {
            case TrajectoryMode.Parabola:
            gameObject.AddComponent<TrajectoryParabola>();
            break;

            case TrajectoryMode.RotateAround:
            gameObject.AddComponent<TrajectoryRotateAround>();
            break;
        }
        projTrajectory = GetComponent<ITrajectory>();

        switch (hitMode)
        {
            case HitMode.Missile:
            gameObject.AddComponent<HitMissile>();
            break;
            
            case HitMode.Grenade:
            gameObject.AddComponent<HitGrenade>();
            break;
        }
        projHit = GetComponent<IHit>();
    }

    public void ProjLaunch(float power) 
    {
        projLaunch.Launch(impulse, power);
    }

}
