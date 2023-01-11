using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryDisplay : MonoBehaviour
{
    [SerializeField] GameObject slotPrefab;
    public InventorySO inventory;
    [SerializeField] Transform firstSlotLocation;

    private void OnEnable() 
    {
        DisplayItems();
    }

    void DisplayItems()
    {
        RectTransform slotRect = slotPrefab.GetComponent<RectTransform>();
        float x = slotRect.sizeDelta.x * 1.25f;
        float y = slotRect.sizeDelta.y * 1.25f;

        RectTransform panelRect = GetComponent<RectTransform>();
        float width = panelRect.sizeDelta.x;
        float height = panelRect.sizeDelta.y;

        int column = 0;
        float currentWidth = 0;

        for (int i = 0; i < inventory.slots.Count ; i++)
        {      
            Vector3 pos = new Vector3(firstSlotLocation.position.x + currentWidth, firstSlotLocation.position.y - (y * column), 0);
  
            Instantiate(slotPrefab, pos, Quaternion.identity, transform);

            currentWidth += x;
            if(width - currentWidth - x < 0)
            {
                currentWidth = 0;
                column ++;
            }
        }
    }
}
