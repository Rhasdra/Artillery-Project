using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_TripleShot : SpawnComponent
{
    public int clones = 2;
    public float offset = 0.3f;

    List<ProjectileManager> children = new List<ProjectileManager>();

    public override void OnSpawn()
    {
        float spawnY = 0f;
        float spawnX = 0f;
        float positive = 1;
        Vector3 position = new Vector3();

        for (int i = 0; i < clones; i++)
        {
            spawnY = transform.position.y + ( transform.up.y * ((offset * positive) + ((int)i/2) * offset * positive));
            spawnX = transform.position.x + (transform.up.x * ((offset * positive) + ((int)i/2) * offset * positive));
            position = new Vector3(spawnX, spawnY, 0);

            GameObject childProj = Instantiate(this.gameObject, position, transform.rotation);
            Destroy(childProj.GetComponent(this.GetType()));
            
            var manager = childProj.GetComponent<ProjectileManager>();
            children.Add(manager);

            positive = -positive;
        }
    }
}
