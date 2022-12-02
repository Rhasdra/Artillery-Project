using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDamageableTo : MonoBehaviour
{
    [SerializeField] GameObject target;
    
    private void OnEnable() {
        target.AddComponent<DamageableNone>();
    }
}
