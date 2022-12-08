using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UI_TurnsDisplayer : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] TurnsManagerEventsChannelSO turnsEvents;

    [SerializeField] TurnsManager turnsManager;
    [SerializeField] TextMeshProUGUI turnsCounter;
    [SerializeField] TextMeshProUGUI charQueue;

    [SerializeField] List<GameObject> listBeingDisplayed;

    private void Awake() 
    {
        
    }

    private void OnEnable() 
    {
        turnsEvents.StartTurn.OnEventRaised += UpdateTurnsCounter;
        turnsEvents.StartTurn.OnEventRaised += UpdateCharQueue;
    }

    private void OnDisable() 
    {
        turnsEvents.StartTurn.OnEventRaised -= UpdateTurnsCounter;
        turnsEvents.StartTurn.OnEventRaised -= UpdateCharQueue;
    }

    void UpdateTurnsCounter()
    {
        turnsCounter.text = (turnsManager.cyclesCounter.ToString() + " : " + turnsManager.turnsCounter.ToString());
    }

    void UpdateCharQueue ()
    {
        if (listBeingDisplayed.Count != 0)
        {
            foreach (GameObject u in listBeingDisplayed)
            {
                Destroy(u);
            }
            listBeingDisplayed.Clear();
        }

        for (int i = 0; i < turnsEvents.charList.Count; i++)
        {
            TeamSO charTeam = turnsEvents.charList[i].team;
            string charName = turnsEvents.charList[i].name.ToString();
            int charDelay = turnsEvents.charList[i].delay;

            TextMeshProUGUI charTMP = Instantiate(charQueue, transform.position, Quaternion.identity);
            charTMP.transform.SetParent(gameObject.transform);
            charTMP.rectTransform.anchoredPosition = new Vector2(charTMP.rectTransform.anchoredPosition.x , charTMP.rectTransform.anchoredPosition.y - (i*charTMP.fontSize*1.2f));

            charTMP.text = ( " - " + charName + " - " + charDelay + "\n");
            charTMP.color = charTeam.color;

            listBeingDisplayed.Add(charTMP.gameObject); 
        }
    }
}
