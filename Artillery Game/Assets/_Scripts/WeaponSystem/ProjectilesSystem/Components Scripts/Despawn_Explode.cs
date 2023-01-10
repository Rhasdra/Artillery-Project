using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn_Explode : DespawnComponent
{
    Collider2D col;

    int explosions = 1;

    protected override void OnEnable() 
    {
        base.OnEnable();
        col = GetComponent<Collider2D>();
    }

    public override void OnDespawn() 
    {
        float explosionRadius = 1; 

        if(manager.victim.gameObject.layer == ((int)Layers.NamesToInt.Characters))
            explosionRadius = 0.5f;

        SpawnExplosion(explosionRadius);

    }
    
    public void SpawnExplosion(float radiusMultiplier)
    {
        if (explosions > 0)
        {
            int random = Random.Range(0, manager.projectileSO.explosions.Length);
            var explosionGO = Instantiate(manager.projectileSO.explosions[random], transform.position, transform.rotation);

            IExplosion exp = explosionGO.GetComponent<IExplosion>();

            explosionGO.SetActive(true);
            exp.Explode(radiusMultiplier);
            
            explosions--;
        }
        else
        {
            return;
        }
    }
}
