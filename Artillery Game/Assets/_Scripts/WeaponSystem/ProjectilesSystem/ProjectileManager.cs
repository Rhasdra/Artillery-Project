using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProjectileManager : MonoBehaviour
{
    [Header("Runtime Set")]
    [SerializeField] GameObjectRuntimeSet projRuntimeSet;

    [Header("Broadcasting To:")]
    [SerializeField] ProjectileEventsChannelSO projectileEvents;

    [Header("Data")]
    public ProjectileSO projectileSO;

    // public List<SpawnComponent> spawnComponents;
    public List<LaunchComponent> launchComponents;
    public List<TrajectoryComponent> trajectoryComponents;
    public List<HitComponent> hitComponents;
    public List<DespawnComponent> despawnComponents;

    public UnityAction<float, float> OnLaunch = delegate { };
    public UnityAction OnTrajectoryTick = delegate { };
    public UnityAction<Collider2D> OnProjectileHit = delegate { };
    public UnityAction OnDespawnProjectile = delegate { };

    bool onTrajectory = false;
    int bounces = 0;
    float power = 0;
    public Collider2D victim = null;

    private void OnEnable() 
    {
        projRuntimeSet.Add(this.gameObject);
    }

    private void OnDisable() 
    {
        projRuntimeSet.Remove(this.gameObject);
    }

    void Start() 
    {
        foreach (ProjectileComponent item in projectileSO.components)
        {
            this.gameObject.AddComponent(item.GetType());
        }

        projectileEvents.SpawnEvent.RaiseEvent(this.gameObject);

        LaunchProjectile(power);
    }

    private void FixedUpdate() 
    {
        if(onTrajectory == true)
        {
            OnTrajectoryTick.Invoke();
            projectileEvents.TrajectoryEvent.RaiseEvent(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        var damageable = other.GetComponent<IDamageable>();
        if(damageable != null)
        {
            OnProjectileHit.Invoke(other);

            if(bounces <= 0)
            {
                OnDespawnProjectile.Invoke();

                projectileEvents.DespawnEvent.RaiseEvent(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    public void RequestLaunch(float _power)
    {
        power = _power;
    }

    public void LaunchProjectile(float power)
    {
        OnLaunch.Invoke(projectileSO.impulse, power);

        projectileEvents.LaunchEvent.RaiseEvent(transform.position);

        onTrajectory = true;
    }

    public void TestButton()
    {
        RequestLaunch(100);
    }
}
