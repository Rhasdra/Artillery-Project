using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ShotSelect : MonoBehaviour
{
    [SerializeField] GameObject togglePrefab;
    [SerializeField] Toggle[] toggles;
    [SerializeField] Weapons currentChar;
    [SerializeField] ToggleGroup group;

    int lastIndex;

    private void Awake()
    {
        group = GetComponent<ToggleGroup>();
    }

    private void OnEnable() 
    {
        TurnsManager.StartTurn.AddListener(GetCurrentChar);
    }

    private void Start() 
    {
        if (currentChar != null)
        {
        SpawnToggles(currentChar.weapons.Length);
        lastIndex = currentChar.index;
        SwapWeapon(currentChar.index);
        }
    }

    void GetCurrentChar()
    {
        currentChar = TurnsManager.currentChar.GetComponent<Weapons>();
        currentChar.SwappedWeapon.AddListener(SwapWeapon); //MIGHT HAVE TO UNSUBSCRIBE AT SOME POINT
    }

    public void SwapWeapon(int i) {
        //Toggle the box
        toggles[i].isOn = true;
    }

    public void ManuallySwapWeapon(int i)
    {
        //Debug.Log("Found the On Toggle! Index: " + i);
        currentChar.index = i;
        currentChar.GetWeapon();
    }

    void SpawnToggles(int number)
    {
        toggles = new Toggle[number];
        if (number > 0)
        {
            for (int i = 0; i < number; i++)
            {
                var newToggle = Instantiate(togglePrefab, transform.position, Quaternion.identity);
                RectTransform rect = newToggle.GetComponent<RectTransform>();
                newToggle.GetComponent<Toggle>().group = group;
                newToggle.GetComponent<UI_WeaponsToggle>().index = i;
                
                Vector3 pos = new Vector3 ( newToggle.transform.position.x + (rect.sizeDelta.x * i * newToggle.transform.localScale.x), newToggle.transform.position.y, newToggle.transform.position.z);
                newToggle.transform.position = pos;

                newToggle.transform.SetParent(this.transform, true);
                toggles[i] = newToggle.GetComponent<Toggle>();             
            }
        }
    }
}