using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableNone : MonoBehaviour , IDamageable
{
    public void TakeDamage(float damage, Vector3 position)
    {
        return;
    }
}
