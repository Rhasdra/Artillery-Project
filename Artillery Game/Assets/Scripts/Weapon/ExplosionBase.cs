using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D.Examples;

public class ExplosionBase : MonoBehaviour, IExplosion
{
    [SerializeField] float baseDamage = 500f;
    [SerializeField] float baseRadius = 1f;
    [SerializeField] float forgiveness = 5f;
    float radius;

    CircleCollider2D myCollider;
    SpriteRenderer mySR;
    D2dExplosion d2d;

    bool debug = false;

    private void OnEnable() 
    {
        myCollider = GetComponent<CircleCollider2D>();
        mySR = GetComponent<SpriteRenderer>();
        d2d = GetComponent<D2dExplosion>();
        transform.localScale = new Vector3 (baseRadius, baseRadius, 1f);
    }

    public void Explode (float radiusMultiplier)
    {   
        radius = radiusMultiplier * baseRadius;
        transform.localScale = new Vector3 (radius, radius, 1f);

        myCollider.enabled = true;
        mySR.enabled = true;
        d2d.enabled = true;

        StartCoroutine("ExplosionCoroutine");
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        var damageable = col.GetComponent<IDamageable>();
        if(damageable == null)
        {
            return;
        }

        //get distance to collider
        Vector2 closestPoint = col.ClosestPoint(transform.position);
        float distance = Vector2.Distance(closestPoint , transform.position);
                            
        //calculate damage based on distance
        float damage = (Mathf.Ceil(baseDamage * (-(distance - radius) / radius)) + forgiveness);
        if(damage > baseDamage)
        {damage = baseDamage;} 
        
        //apply damage
        damageable.TakeDamage(damage);

        if(debug)
        Debug.Log("Hit " + col + " for " + damage + " Explosion Damage");

    }

    IEnumerator ExplosionCoroutine()
    {   
        yield return new WaitForSeconds(1f);
        myCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        Object.Destroy(this.gameObject);
    }
}
