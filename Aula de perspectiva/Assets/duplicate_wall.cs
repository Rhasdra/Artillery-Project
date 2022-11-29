using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class duplicate_wall : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 dimensions;
    public float offset = 1f;      
    
    private void Start() 
    {
        Duplicate();
    }

    private void Duplicate() 
    {
        float spawnY = 0f;
        float spawnX = 0f;
        float positive = 1;
        Vector3 position = new Vector3();

        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            { 
                spawnY = transform.position.y + ( transform.up.y * ((offset * positive) + ((int)y/2) * offset * positive));
                spawnX = transform.position.x + (transform.up.x * ((offset * positive) + ((int)x/2) * offset * positive));
                position = new Vector3(spawnX, spawnY, 0);

                GameObject childProj = Instantiate(this.gameObject, position, transform.rotation);

                positive = -positive;
            }
        }
    }
}
