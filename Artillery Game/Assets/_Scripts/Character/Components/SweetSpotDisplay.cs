using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SweetSpotDisplay : MonoBehaviour
{
    // [SerializeField] GameObject ssPrefab;
    [SerializeField] GameObject ssInstance;
    [SerializeField] CharManager charManager;

    float ssMin = 0;
    float ssMax = 0;

    public void Initialize() 
    {
        charManager = GetComponentInParent<CharManager>();

        
        ssMin = charManager.jobInfo.sweetSpotAngleMin;
        ssMax = charManager.jobInfo.sweetSpotAngleMax;

        // ssInstance = Instantiate(ssPrefab, transform.position, Quaternion.identity);
        ssInstance.transform.localScale = new Vector3(ssInstance.transform.localScale.x * Mathf.Abs(charManager.transform.localScale.x), ssInstance.transform.localScale.y*charManager.transform.localScale.y, ssInstance.transform.localScale.z);
        // ssInstance.transform.SetParent(charManager.transform);

        SweetSpotDisplayManager ssManager;        
        ssManager = ssInstance.GetComponent<SweetSpotDisplayManager>();
        ssManager.SSFill((ssMax-ssMin) / 360);

        ssManager.fill.transform.Rotate(GetRotation(ssMax), Space.Self);

        Display(false);
    }

    Vector3 GetRotation(float ssMax)
    {
        return new Vector3(0, 0, ssMax - 90);
    }

    public void Display(bool display)
    {
        ssInstance.SetActive(display);
    }
}
