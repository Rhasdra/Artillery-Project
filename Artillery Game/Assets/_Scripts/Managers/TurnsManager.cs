using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class TurnsManager : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] CharManagerEventsChannelSO charManagerEvents;

    [Header("Broadcasting to")]
    [SerializeField] TurnsManagerEventsChannelSO eventsChannel;

    [Header("Infos being tracked")]
        [Tooltip("The character which is currently taking their turn.")]
    public static CharManager currentChar; //Should be a ScriptableObject
    public float turnsCounter = 0;
        [Tooltip("In a Cycle, every character in the queue has to take their turn once. Then a new Cycle begins.")]
    public float cyclesCounter = 0;

    [Header("Lists")]
    public List<CharManager> charManagers;
    public static List<CharManager> playersList;
    int index = 0; //tracks which character from the list is the currentChar

    private void OnEnable() 
    {
        charManagerEvents.EndTurn.OnEventRaised += NextCharacter;

        playersList = charManagers;
    }

    private void OnDisable() 
    {
        charManagerEvents.EndTurn.OnEventRaised -= NextCharacter;
    }

    private void Start() 
    {
        currentChar = playersList[index];
        eventsChannel.currentChar = currentChar;
        currentChar.StartListening();
        eventsChannel.StartTurn.OnEventRaised.Invoke();
    }
    
    public void NextCharacter()
    {
        eventsChannel.EndTurn.OnEventRaised.Invoke();

        if ( index < playersList.Count -1)
        {
            index++;
            // return;
        } else {
        index = 0;
        }

        IncreaseTurnsCounter();

        currentChar = playersList[index];
        currentChar.StartListening();

        eventsChannel.StartTurn.OnEventRaised.Invoke();
    }

    void SortQueueByDelay(List<CharManager> unsorted)
    {
        int min;
        CharManager temp;

        for (int i = 0; i < unsorted.Count; i++)
        {
            min = i;

            for (int j = i + 1; j < unsorted.Count; j++)
            {
                if (unsorted[j].delay < unsorted[min].delay)
                {
                    min = j;
                }
                else if (unsorted[j].delay == unsorted[min].delay) //Speed tie
                {
                    if(Random.value >= 0.5)
                    {
                        min = j;
                    }
                }
            }

            if (min != i)
            {
                temp = unsorted[i];
                unsorted[i] = unsorted[min];
                unsorted[min] = temp;
            }
        }
    }

	void Shuffle(List<CharManager> charList)
	{
		// Loops through array
		for (int i = charList.Count-1; i > 0; i--)
		{
			// Randomize a number between 0 and i (so that the range decreases each time)
			int rnd = Random.Range(0,i);
			
			// Save the value of the current i, otherwise it'll overright when we swap the values
			CharManager temp = charList[i];
			
			// Swap the new and old values
			charList[i] = charList[rnd];
			charList[rnd] = temp;
		}
	}

    void IncreaseTurnsCounter()
    {
        turnsCounter ++;
        
        if (turnsCounter % playersList.Count == 0f)
        {
            cyclesCounter ++;
            SortQueueByDelay(playersList);
            eventsChannel.NewCycle.OnEventRaised.Invoke();
        }
    }
}
