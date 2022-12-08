using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class TurnsManager : MonoBehaviour
{
    [Header("Listening to")]
    [SerializeField] CharManagerEventsChannelSO charManagerEvents;
    [SerializeField] BattleManagerEventsChannelSO battleManagerEvents;

    [Header("Broadcasting to")]
    [SerializeField] TurnsManagerEventsChannelSO turnsManagerEvents;

    [Header("Infos being tracked")]
        [Tooltip("The character which is currently taking their turn.")]
    //public CharManager currentChar; //Should be a ScriptableObject
    public float turnsCounter = 0;
        [Tooltip("In a Cycle, every character in the queue has to take their turn once. Then a new Cycle begins.")]
    public float cyclesCounter = 0;
    int index = 0; //tracks which character from the list is the currentChar

    private void Awake() 
    {
        if(turnsManagerEvents.charList.Count > 0f)
            turnsManagerEvents.charList.Clear();
    }

    private void OnEnable() 
    {
        battleManagerEvents.CharacterSpawnEvent.OnEventRaised += AddCharacter;
        battleManagerEvents.SetupFinishEvent.OnEventRaised += Initialize;

        charManagerEvents.EndTurn.OnEventRaised += NextCharacter;
    }

    private void OnDisable() 
    {
        battleManagerEvents.CharacterSpawnEvent.OnEventRaised -= AddCharacter;
        battleManagerEvents.SetupFinishEvent.OnEventRaised -= Initialize;

        charManagerEvents.EndTurn.OnEventRaised -= NextCharacter;
    }

    private void Initialize() 
    {
        Shuffle(turnsManagerEvents.charList);

        turnsManagerEvents.currentChar = turnsManagerEvents.charList[index];
        turnsManagerEvents.currentChar.StartListening();
        turnsManagerEvents.StartTurn.OnEventRaised.Invoke();
    }
    
    public void NextCharacter()
    {
        turnsManagerEvents.EndTurn.OnEventRaised.Invoke();

        if ( index < turnsManagerEvents.charList.Count -1)
        {
            index++;
            // return;
        } else {
        index = 0;
        }

        IncreaseTurnsCounter();

        turnsManagerEvents.currentChar = turnsManagerEvents.charList[index];
        turnsManagerEvents.currentChar.StartListening();

        turnsManagerEvents.StartTurn.OnEventRaised.Invoke();
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
        
        if (turnsCounter % turnsManagerEvents.charList.Count == 0f)
        {
            cyclesCounter ++;
            SortQueueByDelay(turnsManagerEvents.charList);
            turnsManagerEvents.NewCycle.OnEventRaised.Invoke();
        }
    }

    void AddCharacter(GameObject character)
    {
        turnsManagerEvents.charList.Add(character.GetComponent<CharManager>());
    }
}
