using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SweetSpotDisplay : MonoBehaviour
{
    [SerializeField] GameObject ssPrefab;
    CharManager cm;
    SweetSpotDisplayManager ssManager;

    float ssMin = 0;
    float ssMax = 0;

    private void Start() 
    {
        cm = transform.root.GetComponent<CharManager>();
        ssMin = cm.charInfo.sweetSpotAngleMin;
        ssMax = cm.charInfo.sweetSpotAngleMax;

        var ssgo = Instantiate(ssPrefab, transform.position, Quaternion.identity);
        ssgo.GetComponent<Billboard>().followPoint = transform.root.transform;
        
        ssManager = ssgo.GetComponent<SweetSpotDisplayManager>();
        ssManager.SSFill((ssMax-ssMin) / 360);

        ssManager.fill.transform.Rotate(GetRotation(ssMax), Space.Self);
    }

    Vector3 GetRotation(float ssMax)
    {
        return new Vector3(0, 0, ssMax - 90);
    }
}
