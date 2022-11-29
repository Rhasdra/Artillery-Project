using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitMissile : MonoBehaviour, IHit
{
    Collider2D col;
    ProjectileBase projBase;

    IExplosive explosive;

    // RaycastHit2D[] raycastHit;
    float hitAngle;
    Vector3 hitPosition;

    [Header("Debug")]
    [SerializeField] bool debugDist = false;

    RaycastHit2D[] results = new RaycastHit2D[1];

    private void OnEnable() 
    {
        col = GetComponent<Collider2D>();
        projBase = GetComponent<ProjectileBase>();
        explosive = GetComponent<IExplosive>();
    }

    private void Update() 
    {
        col.Raycast(transform.right, results, 1f);
    }

    public float Damage(Collider2D other)
    {
        float damage = 0;

        // Returns hitAngle=0 on 90degrees full hit   
        hitAngle = Vector2.Angle(results[0].normal, -transform.right);
        hitPosition = results[0].point;

        damage = Mathf.RoundToInt((projBase.baseDamage * Mathf.Lerp(1f, 0f, hitAngle/90f)) + projBase.forgiveness);

        // clamps damage to max damage
        if(damage > projBase.baseDamage)
            damage = projBase.baseDamage;

        return damage;
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
            damageable?.TakeDamage(damage);
            projBase.ProjectileHit.Invoke(hitPosition, damage);

            if(debugDist)
            Debug.Log("Hit " + other + " for " + damage + " Missile Damage");
        }
    }
}
