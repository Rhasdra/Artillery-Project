using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileSpawn
{
    void Spawn();
}

public interface IHit
{
    // void GetDamage();
    //void Hit(Collider2D col, float damage);
    void Hit(Collider2D col);
}

public interface IProjectileDespawn
{
    void Despawn();
}


