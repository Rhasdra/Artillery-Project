using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchRotator : LaunchMultiShot
{
    void SetPivot(TrajectoryRotateAround go)
    {
        go.pivotProj = transform;
    }

    public override void SpawnNeighbors(float impulse, float power)
    {
        float spawnY = 0f;
        float spawnX = 0f;
        float positive = 1;
        Vector3 position = new Vector3();

        for (int i = 0; i < quantity; i++)
        {
            //Debug.Log(transform.up);
            spawnY = transform.position.y + ( transform.up.y * ((offset * positive) + ((int)i/2) * offset * positive));
            spawnX = transform.position.x + (transform.up.x * ((offset * positive) + ((int)i/2) * offset * positive));
            position = new Vector3(spawnX, spawnY, 0);

            GameObject childProj = Instantiate(this.gameObject, position, transform.rotation);
            SetPivot(childProj.GetComponent<TrajectoryRotateAround>());
            childProj.GetComponent<LaunchMultiShot>().LaunchAlone(impulse, power);

            positive = -positive;
        }
    }
}
