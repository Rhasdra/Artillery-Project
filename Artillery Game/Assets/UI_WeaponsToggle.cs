using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WeaponsToggle : MonoBehaviour
{
    Clickable clickable;

    public int index;

    private void Awake() 
    {
        clickable = GetComponent<Clickable>();
    }

    private void OnEnable() 
    {
        clickable.LeftClick.AddListener(ForceSwapWeapon);
    }

    public void ForceSwapWeapon()
    {
        transform.parent.GetComponent<UI_ShotSelect>().ManuallySwapWeapon(index);
    }
}
