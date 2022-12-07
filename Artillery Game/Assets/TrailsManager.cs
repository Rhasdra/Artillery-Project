using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailsManager : MonoBehaviour
{
    [Header("Listens to:")]
    [SerializeField] TurnsManagerEventsChannelSO turnsEvents;
    [SerializeField] ProjectileEventsChannelSO projectileEvents;
    
    [Header("Prefab:")]
    [SerializeField] GameObject trailPrefab;

    [Header("Lists:")]
    public List<ProjectileTrail> trailInstances = new List<ProjectileTrail>();

    private void OnEnable() 
    {
        projectileEvents.SpawnEvent.OnEventRaised += SpawnTrail;
        turnsEvents.StartTurn.OnEventRaised += Fade;
    }

    private void OnDisable() 
    {
        projectileEvents.SpawnEvent.OnEventRaised = SpawnTrail;
        turnsEvents.StartTurn.OnEventRaised = Fade;
    }

    void SpawnTrail(GameObject projectile, Vector3 pos, Quaternion rot)
    {
        ProjectileTrail instance = Instantiate(trailPrefab, pos, rot).GetComponent<ProjectileTrail>();
        instance.projectile = projectile.transform;
        instance.owner = turnsEvents.currentChar;

        trailInstances.Add(instance);
    }

    void Fade()
    {
        List<ProjectileTrail> destroy = new List<ProjectileTrail>();

        foreach (ProjectileTrail instance in trailInstances)
        {
            if(instance.owner == turnsEvents.currentChar)
            {
                if (instance.trailLife > 0)
                {
                    instance.trailLife -= 0.5f / TurnsManager.playersList.Count;
                    instance.lineRenderer.startColor = new Color(instance.lineRenderer.startColor.r, instance.lineRenderer.startColor.g, instance.lineRenderer.startColor.b, instance.trailLife);
                    instance.lineRenderer.endColor = new Color(instance.lineRenderer.endColor.r, instance.lineRenderer.endColor.g, instance.lineRenderer.endColor.b, instance.trailLife);

                }else{
                    destroy.Add(instance);
                }
            }
        }

        foreach (var item in destroy)
        {
            trailInstances.Remove(item);
            Destroy(item.gameObject);
        }
    }
}
