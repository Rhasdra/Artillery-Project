using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fill;
    public CharManager owner;

    private void OnEnable() 
    {
        owner.Death.AddListener(DeleteThis);
    }

    void DeleteThis()
    {
        Destroy(this.gameObject);
    }
}
