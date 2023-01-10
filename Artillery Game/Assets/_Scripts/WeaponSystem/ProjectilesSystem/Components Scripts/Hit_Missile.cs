using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Missile : HitComponent
{
    Collider2D col;
    ProjectileSO projSO;

    float hitAngle;
    Vector3 hitPosition;

    RaycastHit2D[] results = new RaycastHit2D[1];

    int layerMask;

    protected override void OnEnable() 
    {
        base.OnEnable();
        
        col = GetComponent<Collider2D>();
        projSO = manager.projectileSO;

        layerMask =~ LayerMask.GetMask(Layers.Wind);
    }

    private void Update() 
    {
        col.Raycast(transform.right, results, 0.3f, layerMask);
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
            damageCalc = projSO.forgiveness;
        }else{
            damageCalc = Mathf.RoundToInt((projSO.baseDamage * Mathf.Lerp(1f, 0f, hitAngle/90f)) + projSO.forgiveness);
        }

        // clamps damage to max damage
        if(damageCalc > projSO.baseDamage)
            damageCalc = projSO.baseDamage;

        return damageCalc;
    }

    public override void Hit(Collider2D other)
    {
        manager.victim = other;
        var damageable = other.GetComponent<IDamageable>();

        if(other.gameObject.CompareTag(Tags.Hurtbox))
        {
            damageable?.TakeDamage(Damage(other), transform.position);
        }
    }
}
