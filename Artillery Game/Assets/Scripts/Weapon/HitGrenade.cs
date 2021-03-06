using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGrenade : MonoBehaviour, IHit
{
    Collider2D col;
    ProjectileBase projBase;
    GameObject explosion;
    
    [Header("Debug")]
    [SerializeField] bool debugDmg = true;
    //[SerializeField] bool debugHit = false;

    private void OnEnable() 
    {
        col = GetComponent<Collider2D>();
        projBase = GetComponent<ProjectileBase>();
        explosion = projBase.explosion;
    }

    public void GetDamage()
    {

    }

    public void Hit(Collider2D col, float damage)
    {
        if(debugDmg)
        Debug.Log("Hit " + col + " for " + damage + " Grenade Damage");

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

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(col.GetComponent<IDamageable>() != null)
        Hit(col, projBase.baseDamage);
    }

    public void Explode (GameObject explosion, float radiusMultiplier)
    {
        var explosionGO = Instantiate(explosion, transform.position, transform.rotation);
        IExplosion exp = explosionGO.GetComponent<IExplosion>();

        explosionGO.SetActive(true);
        exp.Explode(radiusMultiplier);
    }
}
