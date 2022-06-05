using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMissile : MonoBehaviour, IHit
{
    [SerializeField] Collider2D col;
    Collider2D otherCol;
    GameObject explosion;
    ProjectileBase projBase;

    // RaycastHit2D[] raycastHit;
    public float hitAngle;
    public Vector3 hitPosition;

    [Header("Debug")]
    [SerializeField] bool debugDmg = true;
    [SerializeField] bool debugHit = true;

    private void OnEnable() 
    {
        col = GetComponent<Collider2D>();
        projBase = GetComponent<ProjectileBase>();
        explosion = projBase.explosion;
    }

    public float Damage(Collider2D other)
    {
        float damage = 0f;
        Ray2D ray = new Ray2D(transform.position, transform.right);
        RaycastHit2D[] results = new RaycastHit2D[5];

        col.Raycast(transform.right, results, 1f);

        Debug.DrawRay(transform.position, transform.right, Color.black);

        // chech for the OnTriggerEnterCollider
        for (int i = 0; i < results.Length; i++)
        {
            if ( results[i].collider == other)
            {
                //Returns hitAngle=0 on 90degrees full hit   
                hitAngle = Vector2.Angle(results[i].normal, -transform.right);
                hitPosition = results[i].point;

                damage = Mathf.RoundToInt((projBase.baseDamage * Mathf.Lerp(1f, 0f, hitAngle/90f)) + projBase.forgiveness);

                if(damage > projBase.baseDamage)
                    damage = projBase.baseDamage;

                if(debugHit)
                Debug.Log("Normal: " + results[i].normal + " Angle: " + (90 - hitAngle) + " and Position: " + hitPosition);
            }
        }

        return damage;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        var damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            otherCol = other;
            Hit(other, Damage(other));
        }
           

    }

    public void Explode (GameObject explosion, float radiusMultiplier)
    {
        var explosionGO = Instantiate(explosion, transform.position, transform.rotation);
        IExplosion exp = explosionGO.GetComponent<IExplosion>();

        explosionGO.SetActive(true);
        exp.Explode(radiusMultiplier);
    }

    public void Hit(Collider2D col, float damage)
    {
        if(debugDmg)
        Debug.Log("Hit " + col + " for " + damage + " Missile Damage");

        float expRadius = 1;    
        var damageable = col.GetComponent<IDamageable>();
        
        if(col.CompareTag("Player"))
        {
        damageable?.TakeDamage(damage);
        expRadius = 0.5f;
        }

        Explode(explosion, expRadius);
        Object.Destroy(this.gameObject); 
    }

    public void D2Hit()
    {
        Hit(otherCol, Damage(otherCol));
        Destroy(this);
    }
}
