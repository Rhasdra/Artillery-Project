using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMissile : MonoBehaviour, IHit
{
    Collider2D col;
    GameObject explosion;
    ProjectileBase projBase;

    RaycastHit2D[] raycastHit;
    public float hitAngle;
    public Vector3 hitPosition;
    public float damage;

    [Header("Debug")]
    [SerializeField] bool debugDmg = true;
    [SerializeField] bool debugHit = false;

    private void OnEnable() 
    {
        col = GetComponent<Collider2D>();
        projBase = GetComponent<ProjectileBase>();
        explosion = projBase.explosion;
    }

    private void Update() 
    {
        GetDamage();
    }

    public void GetDamage()
    {
        //int mask =~ LayerMask.GetMask("Projectiles"); // ~ inverts the mask, so it hit anything but this one
        raycastHit = Physics2D.RaycastAll (transform.position, transform.right, 0.3f);

        Debug.DrawRay(transform.position, transform.right, Color.black);

        for (int i = 0; i < raycastHit.Length; i++)
        {
            var damageable = raycastHit[i].collider.GetComponent<IDamageable>();

            //Returns hitAngle=0 on 90degrees full hit
            if(damageable != null)
            {    
            hitAngle = Vector2.Angle(raycastHit[i].normal, -transform.right);
            hitPosition = raycastHit[i].point;
            damage = Mathf.RoundToInt((projBase.baseDamage * Mathf.Lerp(1f, 0f, hitAngle/90f)) + projBase.forgiveness);
            
            if(damage > projBase.baseDamage)
                damage = projBase.baseDamage;

            if(debugHit)
            Debug.Log(raycastHit[i].collider + " " + hitAngle + " " + hitPosition);
        }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        var damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            Hit(other, damage);
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
}
