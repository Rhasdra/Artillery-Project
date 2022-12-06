using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitMissile : MonoBehaviour, IHit
{
    Collider2D col;
    Projectile projBase;

    IExplosive explosive;

    float hitAngle;
    Vector3 hitPosition;

    RaycastHit2D[] results = new RaycastHit2D[1];

    private void OnEnable() 
    {
        col = GetComponent<Collider2D>();
        projBase = GetComponent<Projectile>();
        explosive = GetComponent<IExplosive>();
    }

    private void Update() 
    {
        col.Raycast(transform.right, results, 0.3f);
    }

    public float Damage(Collider2D other)
    {
        float damageCalc = 0;

        // Returns hitAngle=0 on 90degrees full hit   
        hitAngle = Vector2.Angle(results[0].normal, -transform.right);
        hitPosition = results[0].point;

        //It also returns 0 when raycast hits nothing, so this if statement solves that
        if(hitAngle == 0)
        {
            damageCalc = projBase.forgiveness;
        }else{
            damageCalc = Mathf.RoundToInt((projBase.baseDamage * Mathf.Lerp(1f, 0f, hitAngle/90f)) + projBase.forgiveness);
        }

        // clamps damage to max damage
        if(damageCalc > projBase.baseDamage)
            damageCalc = projBase.baseDamage;

        return damageCalc;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        var damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            Hit(other, Damage(other));
            explosive?.Explode(other);
        }
    }

    public void Hit(Collider2D other, float damage)
    {
   
        var damageable = other.GetComponent<IDamageable>();
        
        if(other.gameObject.CompareTag("Hurtbox"))
        {
            damageable?.TakeDamage(damage, transform.position);
        }

        //Broadcast the Event
        GetComponent<Projectile>().projectileEvents.HitEvent.OnEventRaised(transform.position, (int)damage, damageable);
    }
}
