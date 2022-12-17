using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Destructible2D.Examples;

public class Explosion : MonoBehaviour, IExplosion
{
    [SerializeField] float baseDamage = 500f;
    [SerializeField] float radius = 1f;
    [SerializeField] float forgiveness = 50f;

    [SerializeField] float explosionForce = 5f;

    [SerializeField] float lifeSeconds = 0.5f; 

    CircleCollider2D myCollider;
    SpriteRenderer mySR;
    D2dExplosion d2d;
    List<Collider2D> alreadyHit = new List<Collider2D>();

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

            //Knockback
            Knockback(other, closestPoint);

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

    void Knockback(Collider2D other, Vector2 contactPoint) //Way too buggy
    
    {
        if(alreadyHit.Contains(other))
            return;

        Rigidbody2D rb = other.GetComponentInParent<Rigidbody2D>();
        if(rb != null)
        {
            //// Unfreeze Z Rotation
            // var headsUp = rb.GetComponent<HeadsUp>();
            //     if(headsUp != null)
            //     {
            //         rb.constraints = RigidbodyConstraints2D.None;
            //         StartCoroutine(headsUp.Stun(stunSeconds));
            //     }
            //AddExplosionForce(rb, transform.position, contactPoint, explosionForce);
            Rigidbody2DExtensions.AddExplosionForce(rb, explosionForce, transform.position, radius, 0f, ForceMode2D.Impulse);
        }

        alreadyHit.Add(other);
    }

    // this Rigidbody2D rb, float explosionForce, 
    // Vector2 explosionPosition, float explosionRadius, 
    // float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force

    // void AddExplosionForce(Rigidbody2D rb, Vector2 pos, Vector2 contactPoint, float explosionForce, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Impulse)
    // {
    //     var explosionDir = rb.position - pos;
    //     var explosionDistance = contactPoint - (Vector2)transform.position;
    //     float wearoff = 1 - (explosionDistance.magnitude - radius);

    //     // Normalize without computing magnitude again
    //     if (upwardsModifier == 0)
    //         {
    //             explosionDir /= explosionDistance;
    //         }
    //     else 
    //     {
    //         // From Rigidbody.AddExplosionForce doc:
    //         // If you pass a non-zero value for the upwardsModifier parameter, the direction
    //         // will be modified by subtracting that value from the Y component of the centre point.
    //         explosionDir.y += upwardsModifier;
    //         explosionDir.Normalize();
    //     }

    //     // Unfreeze Z Rotation
    //     var headsUp = rb.GetComponent<HeadsUp>();
    //         if(headsUp != null)
    //         {
    //             rb.constraints = RigidbodyConstraints2D.None;
    //             StartCoroutine(headsUp.Stun(stunSeconds));
    //         }
            


    //     float explosionPower = Mathf.Lerp(0, explosionForce, ((radius - explosionDistance) / radius));
    //     rb.AddForce(explosionPower * explosionDir, mode);
        
    //     float torquePower = (torqueForce * -Mathf.Sign(explosionDir.x) * ((radius - explosionDistance) / radius));
    //     rb.AddTorque(torquePower, mode);

    //     Debug.Log(explosionPower);

            
    // }
}
