using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDataTest : MonoBehaviour
{
    public CharDataSO charData;
    public JobSO job;

    public GameObject prefab;
    
    public void CreateNewCharData()
    {
        StartCoroutine(CreateCoroutine());
    }

    public IEnumerator CreateCoroutine()
    {
        charData = ScriptableObject.CreateInstance(typeof(CharDataSO)) as CharDataSO;
        charData.Init(job);

        CharManager instance = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<CharManager>();
        instance.charData = charData;

        yield return new WaitForSeconds(1f);
        
        instance.StartTurn();
    }
}
