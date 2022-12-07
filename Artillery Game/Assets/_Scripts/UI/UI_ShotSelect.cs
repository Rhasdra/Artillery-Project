using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ShotSelect : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] TurnsManagerEventsChannelSO turnsManagerEvents;
    [SerializeField] WeaponEventsChannelSO weaponEvents;
    
    [SerializeField] GameObject togglePrefab;
    [SerializeField] List<Toggle> toggles;
    [SerializeField] WeaponsManager currentChar;
    [SerializeField] ToggleGroup group;

    int lastIndex;

    private void Awake()
    {
        group = GetComponent<ToggleGroup>();
    }

    private void OnEnable() 
    {
        turnsManagerEvents.StartTurn.OnEventRaised += GetCurrentChar;
        weaponEvents.WeaponChangeIndexEvent.OnEventRaised += SwapWeapon;
    }

    private void OnDisable() 
    {
        turnsManagerEvents.StartTurn.OnEventRaised -= GetCurrentChar;
        weaponEvents.WeaponChangeIndexEvent.OnEventRaised -= SwapWeapon;
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
        currentChar = turnsManagerEvents.currentChar.GetComponent<WeaponsManager>();
        SpawnToggles(currentChar.weapons.Length);
        SwapWeapon(currentChar.index);
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
        if (toggles != null)
        {
            foreach (var item in toggles)
            {
                Destroy(item.gameObject);
            }
        }

        toggles = new List<Toggle>();
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
                toggles.Add(newToggle.GetComponent<Toggle>());             
            }
        }
    }
}
