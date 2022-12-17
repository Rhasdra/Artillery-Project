using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    [Header("Listening to:")]
    [SerializeField] ProjectileEventsChannelSO projectileEvents;
    [SerializeField] HealthEventsChannelSO healthEvents;

    [Header("Broadcasting to")]
    [SerializeField] BattleManagerEventsChannelSO battleManagerEvents;

    [Header("Runtime Sets")]
    [SerializeField] GameObjectRuntimeSet charactersRuntimeSet;

    [SerializeField] List<TeamSO> teams;
    [SerializeField] GameObject charLabel = null;
    [SerializeField] GameObject projLabel = null;

    [SerializeField] GameObject terrain;
    [SerializeField] float width;
    [SerializeField] float height = 5f;

    [SerializeField] GameObject winScreenPrefab;
    private void OnEnable() 
    {
        width = terrain.GetComponentInChildren<SpriteRenderer>().bounds.size.x * 0.8f;

        projectileEvents.SpawnEvent.OnEventRaised += AddProjectile;
        projectileEvents.DespawnEvent.OnEventRaised += RemoveProjectile;
        healthEvents.CharacterDeath.OnEventRaised += CheckTeams;
    }

    void OnDisable()
    {
        projectileEvents.SpawnEvent.OnEventRaised -= AddProjectile;
        projectileEvents.DespawnEvent.OnEventRaised -= RemoveProjectile;
        healthEvents.CharacterDeath.OnEventRaised -= CheckTeams;

        //Clean itself
        battleManagerEvents.Projectiles.Clear();
    }

    private void Start() 
    {
        charLabel = new GameObject();
        charLabel.name = "------- CHARACTERS -------";
        projLabel = new GameObject();
        projLabel.name = "------- PROJECTILES -------";

        SpawnTeams();
        battleManagerEvents.Projectiles.Clear();
    }

    void SpawnTeams()
    {
        //battleManagerEvents.Characters.Clear();
        charactersRuntimeSet.Clear();

        StartCoroutine(SpawnTeamsCoroutine());          
    }

    Vector3 RandomLocation()
    {
        bool groundCheck = false;
        float x = 0;
        
        while (groundCheck == false)
        {
            x = Random.Range(-(width/2) , (width/2));

            RaycastHit2D ray = Physics2D.Raycast (new Vector2(x, height), -Vector2.up, Mathf.Infinity, LayerMask.GetMask(Layers.Terrain, Layers.Characters));

            if (ray.collider != null && ray.collider.gameObject.layer != LayerMask.NameToLayer(Layers.Characters))
                {
                    groundCheck = true;
                }
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
                instance.transform.parent = charLabel.transform;
                var instManager = instance.GetComponent<CharManager>();
                instManager.team = team;
                
                if(instance.transform.position.x > 0)
                    instance.transform.localScale = new Vector3(-instance.transform.localScale.x, instance.transform.localScale.y, instance.transform.localScale.z);

                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(1f);

        battleManagerEvents.SetupFinishEvent.OnEventRaised.Invoke();
    }

    void AddProjectile (GameObject proj)
    {
        battleManagerEvents.Projectiles.Add(proj);
        proj.transform.parent = projLabel.transform;
    }

    void RemoveProjectile (GameObject proj)
    {
        battleManagerEvents.Projectiles.Remove(proj);

        if(battleManagerEvents.Projectiles.Count == 0)
            battleManagerEvents.EmptyProjectileList.OnEventRaised.Invoke();
    }


    void CheckTeams(GameObject go = null)
    {
        List<TeamSO> remove = new List<TeamSO>();

        foreach (var team in teams)
        {
            int alive = 0;

            foreach (var character in charactersRuntimeSet.Items)
            {
                if (character.GetComponent<CharManager>().team == team)
                alive ++;
            }

            if(alive == 0)
            {
                remove.Add(team);
            }
        }

        foreach (var team in remove)
        {
            RemoveTeam(team);
        }
    }

    void RemoveTeam(TeamSO team)
    {
        teams.Remove(team);

        if(teams.Count == 1)
        {
            EndGame(teams[0]);
        }
    }

    void EndGame(TeamSO winner)
    {
        var instance = Instantiate(winScreenPrefab, Vector3.zero, Quaternion.identity);
        var parent = GameObject.FindGameObjectWithTag("Canvas");
        instance.transform.position = parent.transform.position;
        instance.transform.SetParent(parent.transform);
        battleManagerEvents.EndBattleEvent.RaiseEvent();
    }
}
