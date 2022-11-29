using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D.Examples;

public class ExplosionBase : MonoBehaviour, IExplosion
{
    [SerializeField] float baseDamage = 500f;
    [SerializeField] float radius = 1f;
    [SerializeField] float forgiveness = 5f;
    //float radius;

    [SerializeField] float lifeSeconds = 0.5f; 

    CircleCollider2D myCollider;
    SpriteRenderer mySR;
    D2dExplosion d2d;

    [SerializeField] bool debug = false;

    private void OnEnable() 
    {
        myCollider = GetComponent<CircleCollider2D>();
        mySR = GetComponentInChildren<SpriteRenderer>();
        d2d = GetComponent<D2dExplosion>();
    }

    float newRadius(float radiusMultiplier)
    {
        return radiusMultiplier * radius;
    }

    public void Explode (float radiusMultiplier)
    {   
        transform.localScale = new Vector3 (newRadius(radiusMultiplier), newRadius(radiusMultiplier), 1f);
        d2d.StampSize = new Vector2 (newRadius(radiusMultiplier)*2, newRadius(radiusMultiplier)*2);

        mySR.enabled = true;
        myCollider.enabled = true;

        StartCoroutine("ExplosionCoroutine");
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        var damageable = other.GetComponent<IDamageable>();
        if(damageable == null)
        {return;}

        if(other.gameObject.CompareTag("Hurtbox"))
        {
            //get distance to collider
            Vector2 closestPoint = other.ClosestPoint(transform.position);
            float distance = Vector2.Distance(closestPoint , transform.position);
                                
            //calculate damage based on distance
            //float damage = (Mathf.Ceil(baseDamage * (-((distance-1) - radius) / radius)) + forgiveness);
            float damage = Mathf.Ceil((baseDamage * (radius - distance) / radius) + forgiveness);
            if(damage > baseDamage)
            {damage = baseDamage;} 
            
            //apply damage
            damageable.TakeDamage(damage, transform.position);

            if(debug)
            Debug.Log("Hit " + other + " for " + damage + " Explosion Damage. Distance from impact: " + distance);
        }
    }

    IEnumerator ExplosionCoroutine()
    {   
        yield return
        d2d.enabled = true;
        yield return new WaitForSeconds(lifeSeconds);
        myCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Object.Destroy(this.gameObject);
    }
}
