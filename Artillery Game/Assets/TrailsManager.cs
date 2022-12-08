using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailsManager : MonoBehaviour
{
    [Header("Listens to:")]
    [SerializeField] BattleManagerEventsChannelSO battleEvents;
    [SerializeField] TurnsManagerEventsChannelSO turnsEvents;
    [SerializeField] ProjectileEventsChannelSO projectileEvents;
    
    [Header("Prefab:")]
    [SerializeField] GameObject trailPrefab;

    [Header("Lists:")]
    public List<ProjectileTrail> trailInstances = new List<ProjectileTrail>();

    [Header("Settings:")]
    [SerializeField] float startingAlpha = 0.25f;
    
    [Header("Current Trail:")]
    ProjectileTrail currentTrail = null;
    Color currentTrailStartColor = Color.black;
    Color currentTrailEndColor = Color.black;
    [SerializeField] float currentTrailWidth = 0f;

    private void OnEnable() 
    {
        projectileEvents.SpawnEvent.OnEventRaised += SpawnTrail;
        
        turnsEvents.StartTurn.OnEventRaised += Fade;
        turnsEvents.StartTurn.OnEventRaised += Highlight;

        turnsEvents.EndTurn.OnEventRaised += CancelHighlight;
    }

    private void OnDisable() 
    {
        projectileEvents.SpawnEvent.OnEventRaised -= SpawnTrail;
        
        turnsEvents.StartTurn.OnEventRaised -= Fade;
        turnsEvents.StartTurn.OnEventRaised -= Highlight;

        turnsEvents.EndTurn.OnEventRaised -= CancelHighlight;
    }

    void SpawnTrail(GameObject projectile)
    {
        ProjectileTrail instance = Instantiate(trailPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<ProjectileTrail>();
        instance.transform.parent = this.transform;
        instance.projectile = projectile.transform;
        instance.owner = turnsEvents.currentChar;
        
        //Set Starting Alpha
        instance.trailLife = startingAlpha;
        // instance.lineRenderer.startColor = new Color(instance.lineRenderer.startColor.r, instance.lineRenderer.startColor.g, instance.lineRenderer.startColor.b, startingAlpha);
        // instance.lineRenderer.endColor = new Color(instance.lineRenderer.endColor.r, instance.lineRenderer.endColor.g, instance.lineRenderer.endColor.b, startingAlpha);        

        trailInstances.Add(instance);
    }

    void Fade()
    {
        List<ProjectileTrail> destroy = new List<ProjectileTrail>();

        foreach (ProjectileTrail instance in trailInstances)
        {
            if (instance.trailLife > 0)
            {
                instance.trailLife -= (startingAlpha/2) / turnsEvents.charList.Count; //dissapears in 2 turns
                instance.lineRenderer.startColor = new Color(instance.lineRenderer.startColor.r, instance.lineRenderer.startColor.g, instance.lineRenderer.startColor.b, instance.trailLife);
                instance.lineRenderer.endColor = new Color(instance.lineRenderer.endColor.r, instance.lineRenderer.endColor.g, instance.lineRenderer.endColor.b, instance.trailLife);

            }else{
                destroy.Add(instance);
            }

        }

        foreach (var item in destroy)
        {
            trailInstances.Remove(item);
            Destroy(item.gameObject);
        }
    }

    void Highlight()
    {
        if(trailInstances == null)
        return;

        //Find Trails made by current character
        List<ProjectileTrail> currentTrails = new List<ProjectileTrail>();
        foreach (ProjectileTrail trail in trailInstances)
        {
            if (trail.owner == turnsEvents.currentChar)
            {
                currentTrails.Add(trail);
            }
        }
        
        //Get the most recent one
        currentTrail = null;
        float mostRecentTime = 0;
        foreach (ProjectileTrail trail in currentTrails)
        {
            if(trail.timeOfCreation > mostRecentTime)
            {
                currentTrail = trail;
                mostRecentTime = trail.timeOfCreation;
            }
        }
        

        if (currentTrail == null)
            return;
        //Save previous values
        currentTrailStartColor = currentTrail.lineRenderer.startColor;
        currentTrailEndColor = currentTrail.lineRenderer.endColor;
        currentTrailWidth = currentTrail.lineRenderer.startWidth;
    
        //Highlight it
        currentTrail.lineRenderer.startColor = Color.white;
        currentTrail.lineRenderer.endColor = Color.white;
        currentTrail.lineRenderer.startWidth *= 1.5f;
        currentTrail.lineRenderer.endWidth *= 1.5f;
    }

    void CancelHighlight()
    {
        if (currentTrail == null)
            return;

        currentTrail.lineRenderer.startColor = currentTrailStartColor;
        currentTrail.lineRenderer.endColor = currentTrailEndColor;
        currentTrail.lineRenderer.startWidth = currentTrailWidth;
        currentTrail.lineRenderer.endWidth = currentTrailWidth;
    }
}
