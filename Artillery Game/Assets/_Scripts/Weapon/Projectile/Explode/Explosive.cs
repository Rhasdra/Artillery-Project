using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour, IExplosive
{
    Collider2D col;
    GameObject explosion;
    Projectile projBase;

    int explosions = 1;

     private void OnEnable() 
    {
        col = GetComponent<Collider2D>();
        projBase = GetComponent<Projectile>();
        explosion = projBase.explosion;
    }

    public void Explode(Collider2D other) 
    {
        float explosionRadius = 1; 
        var damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            if (other.gameObject.CompareTag("Hurtbox"))
            {
                explosionRadius = 0.5f;
            }

            SpawnExplosion(explosionRadius, col);
        }
    }
    
    public void SpawnExplosion(float radiusMultiplier, Collider2D col)
    {
        if (explosions > 0)
        {
        var explosionGO = Instantiate(explosion, transform.position, transform.rotation);
        IExplosion exp = explosionGO.GetComponent<IExplosion>();

        explosionGO.SetActive(true);
        exp.Explode(radiusMultiplier);
        
        explosions--;
        }
        else
        {
            return;
        }

        Destroy(this.gameObject);
    }
}
