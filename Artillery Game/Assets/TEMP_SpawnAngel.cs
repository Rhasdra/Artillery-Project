using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_SpawnAngel : MonoBehaviour
{
    [SerializeField] GameObject angelPrefab;

    public void SpawnAngel()
    {
        Instantiate(angelPrefab, transform.position, Quaternion.identity);
    }
}
