using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileSpawn
{
    void OnSpawn();
}

public interface IHit
{
    void Hit(Collider2D col);
}

public interface ILaunch
{
    void Launch(float impulse, float power);
}

public interface ITrajectory
{
    void UpdateTrajectory();
}

public interface IProjectileDespawn
{
    void OnDespawn();
}


