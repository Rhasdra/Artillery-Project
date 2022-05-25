using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    void GetDamage();
    void Hit(Collider2D col, float damage);
}
