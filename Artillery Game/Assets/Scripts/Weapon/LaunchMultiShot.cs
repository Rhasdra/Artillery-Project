using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMultiShot : MonoBehaviour, ILaunch
{
    public int quantity = 4;
    public float offset = 0.3f;

    Rigidbody2D rb;

    void OnEnable() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(float impulse, float power)
    {
       MultiShot(impulse, power);
    }

    public void LaunchAlone(float impulse, float power)
    {
        rb.AddForce(transform.right * impulse * (power/100f));
    }

    public virtual void SpawnNeighbors(float impulse, float power)
    {
        float spawnY = 0f;
        float spawnX = 0f;
        float positive = 1;
        Vector3 position = new Vector3();

        for (int i = 0; i < quantity; i++)
        {
            Debug.Log(transform.up);
            spawnY = transform.position.y + ( transform.up.y * ((offset * positive) + ((int)i/2) * offset * positive));
            spawnX = transform.position.x + (transform.up.x * ((offset * positive) + ((int)i/2) * offset * positive));
            position = new Vector3(spawnX, spawnY, 0);

            GameObject childProj = Instantiate(this.gameObject, position, transform.rotation);
            childProj.GetComponent<LaunchMultiShot>().LaunchAlone(impulse, power);

            positive = -positive;
        }
    }

    public virtual void MultiShot(float impulse, float power)
    {
        LaunchAlone(impulse, power);
        SpawnNeighbors(impulse, power);
    }
}
