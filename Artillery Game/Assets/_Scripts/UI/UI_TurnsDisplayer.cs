using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UI_TurnsDisplayer : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] TurnsManagerEventsChannelSO turnsManagerEvents;

    [SerializeField] TurnsManager turnsManager;
    [SerializeField] TextMeshProUGUI turnsCounter;
    [SerializeField] TextMeshProUGUI charQueue;

    [SerializeField] List<GameObject> listBeingDisplayed;

    private void Awake() 
    {
        
    }

    private void OnEnable() 
    {
        turnsManagerEvents.StartTurn.OnEventRaised += UpdateTurnsCounter;
        turnsManagerEvents.StartTurn.OnEventRaised += UpdateCharQueue;
    }

    private void OnDisable() 
    {
        turnsManagerEvents.StartTurn.OnEventRaised -= UpdateTurnsCounter;
        turnsManagerEvents.StartTurn.OnEventRaised -= UpdateCharQueue;
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

        for (int i = 0; i < TurnsManager.playersList.Count; i++)
        {
            //TeamSO charTeam = turnsManager.charManagers[i].GetComponent<CharManager>().charInfo.team;
            string charName = TurnsManager.playersList[i].name.ToString();
            int charDelay = TurnsManager.playersList[i].delay;

            TextMeshProUGUI charTMP = Instantiate(charQueue, transform.position, Quaternion.identity);
            charTMP.transform.SetParent(gameObject.transform);
            charTMP.rectTransform.anchoredPosition = new Vector2(charTMP.rectTransform.anchoredPosition.x , charTMP.rectTransform.anchoredPosition.y - (i*charTMP.fontSize*1.2f));

            charTMP.text = ( " - " + charName + " - " + charDelay + "\n");
            charTMP.color = Color.white;

            listBeingDisplayed.Add(charTMP.gameObject); 
        }
    }
}
