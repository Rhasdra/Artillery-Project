using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHit
{
    // void GetDamage();
    //void Hit(Collider2D col, float damage);
    void Hit(Collider2D col);
}
