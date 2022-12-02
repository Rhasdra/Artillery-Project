using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMultiShot : LaunchImpulse
{
    public int quantity = 4;
    public float offset = 0.3f;

    public override void Launch(float impulse, float power)
    {
        base.Launch(impulse, power);
        SpawnNeighbors(impulse, power);
    }

    public void LaunchAlone(float impulse, float power)
    {
        rb.AddForce(transform.right * impulse * (power/100f));
        //Broadcast the Event
        GetComponent<Projectile>().projectileEvents.LaunchEvent.OnEventRaised(transform.position);
    }

    public virtual void SpawnNeighbors(float impulse, float power)
    {
        float spawnY = 0f;
        float spawnX = 0f;
        float positive = 1;
        Vector3 position = new Vector3();

        for (int i = 0; i < quantity; i++)
        {
            spawnY = transform.position.y + ( transform.up.y * ((offset * positive) + ((int)i/2) * offset * positive));
            spawnX = transform.position.x + (transform.up.x * ((offset * positive) + ((int)i/2) * offset * positive));
            position = new Vector3(spawnX, spawnY, 0);

            GameObject childProj = Instantiate(this.gameObject, position, transform.rotation);
            childProj.GetComponent<LaunchMultiShot>().LaunchAlone(impulse, power);

            positive = -positive;
        }
    }
}
