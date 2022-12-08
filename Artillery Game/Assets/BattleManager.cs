using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    [Header("Listening to:")]
    [SerializeField] ProjectileEventsChannelSO projectileEvents;

    [Header("Broadcasting to")]
    [SerializeField] BattleManagerEventsChannelSO eventsChannel;

    [SerializeField] List<TeamSO> teams;

    [SerializeField] GameObject terrain;
    [SerializeField] float width;
    [SerializeField] float height = 5f;

    private void OnEnable() 
    {
        width = terrain.GetComponentInChildren<SpriteRenderer>().bounds.size.x;

        projectileEvents.SpawnEvent.OnEventRaised += AddProjectile;
        projectileEvents.DespawnEvent.OnEventRaised += RemoveProjectile;
    }

    private void Start() 
    {
        SpawnTeams();
        eventsChannel.Projectiles.Clear();
    }

    void SpawnTeams()
    {
        eventsChannel.Characters.Clear();

        StartCoroutine(SpawnTeamsCoroutine());          
    }

    Vector3 RandomLocation()
    {
        bool groundCheck = false;
        float x = 0;
        
        while (groundCheck == false)
        {
            x = Random.Range(-(width/2) , (width/2));
            RaycastHit2D ray = Physics2D.Raycast (new Vector2(x, height), -Vector2.up, Mathf.Infinity, LayerMask.GetMask("Terrain"));

            if (ray.collider != null)
                groundCheck = true;
        }

        return new Vector3(x, height, 0f);
    }

    IEnumerator SpawnTeamsCoroutine()
    {
        foreach (var team in teams)
        {
            foreach (var character in team.characters)
            {
                var instance = Instantiate(character, RandomLocation(), Quaternion.identity);
                instance.GetComponent<CharManager>().team = team;
                
                if(instance.transform.position.x > 0)
                    instance.transform.localScale = new Vector3(-instance.transform.localScale.x, instance.transform.localScale.y, instance.transform.localScale.z);

                eventsChannel.CharacterSpawnEvent.OnEventRaised.Invoke(instance);
                eventsChannel.Characters.Add(instance);

                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(1f);

        eventsChannel.SetupFinishEvent.OnEventRaised.Invoke();
    }

    void AddProjectile (GameObject proj)
    {
        eventsChannel.Projectiles.Add(proj);
    }

    void RemoveProjectile (GameObject proj)
    {
        eventsChannel.Projectiles.Remove(proj);

        if(eventsChannel.Projectiles.Count == 0)
            eventsChannel.EmptyProjectileList.OnEventRaised.Invoke();
    }
}
