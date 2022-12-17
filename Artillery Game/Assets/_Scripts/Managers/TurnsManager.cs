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

    [Header("Runtime Set")]
    [SerializeField] GameObjectRuntimeSet charactersRuntimeSet;

    [Header("Infos being tracked")]
        [Tooltip("The character which is currently taking their turn.")]
    //public CharManager currentChar; //Should be a ScriptableObject
    public float turnsCounter = 0;
        [Tooltip("In a Cycle, every character in the queue has to take their turn once. Then a new Cycle begins.")]
    public float cyclesCounter = 0;
    int index = 0; //tracks which character from the list is the currentChar

    private void OnEnable() 
    {
        battleManagerEvents.SetupFinishEvent.OnEventRaised += Setup;

        charManagerEvents.EndTurn.OnEventRaised += NextCharacter;

        battleManagerEvents.EndBattleEvent.OnEventRaised += EndBattle;
    }

    private void OnDisable() 
    {
        charactersRuntimeSet.OnItemAdd.OnEventRaised -= UpdateList;
        charactersRuntimeSet.OnItemRemove.OnEventRaised -= UpdateList;
        battleManagerEvents.SetupFinishEvent.OnEventRaised -= Setup;

        charManagerEvents.EndTurn.OnEventRaised -= NextCharacter;

        battleManagerEvents.EndBattleEvent.OnEventRaised -= EndBattle;

        // Clear
        if(turnsManagerEvents.charTakingTurn != null)
            turnsManagerEvents.charTakingTurn = null;
        if(turnsManagerEvents.charList != null)
            turnsManagerEvents.charList.Clear();
    }

    private void Setup() 
    {
        charactersRuntimeSet.OnItemAdd.OnEventRaised += UpdateList;
        charactersRuntimeSet.OnItemRemove.OnEventRaised += UpdateList;

        
        Shuffle(charactersRuntimeSet.Items);


        turnsManagerEvents.charTakingTurn = charactersRuntimeSet.Items[index].GetComponent<CharManager>();
        turnsManagerEvents.charTakingTurn.StartListening();
        turnsManagerEvents.SetupFinishEvent.RaiseEvent();
        turnsManagerEvents.StartTurn.RaiseEvent();
    }
    
    public void NextCharacter()
    {
        turnsManagerEvents.EndTurn.OnEventRaised.Invoke();

        if ( index < charactersRuntimeSet.Items.Count -1)
        {
            index++;
            // return;
        } else {
        index = 0;
        }

        IncreaseTurnsCounter();

        turnsManagerEvents.charTakingTurn = charactersRuntimeSet.Items[index].GetComponent<CharManager>();
        turnsManagerEvents.charTakingTurn.StartListening();

        turnsManagerEvents.StartTurn.RaiseEvent();
    }

    void SortQueueByDelay(List<GameObject> unsorted)
    {
        int min;
        CharManager temp;

        for (int i = 0; i < unsorted.Count; i++)
        {
            min = i;

            for (int j = i + 1; j < unsorted.Count; j++)
            {
                if (unsorted[j].GetComponent<CharManager>().delay < unsorted[min].GetComponent<CharManager>().delay)
                {
                    min = j;
                }
                else if (unsorted[j].GetComponent<CharManager>().delay == unsorted[min].GetComponent<CharManager>().delay) //Speed tie
                {
                    if(Random.value >= 0.5)
                    {
                        min = j;
                    }
                }
            }

            if (min != i)
            {
                temp = unsorted[i].GetComponent<CharManager>();
                unsorted[i] = unsorted[min];
                unsorted[min] = temp.gameObject;
            }
        }
    }

	void Shuffle(List<GameObject> charList)
	{
		// Loops through array
		for (int i = charList.Count-1; i > 0; i--)
		{
			// Randomize a number between 0 and i (so that the range decreases each time)
			int rnd = Random.Range(0,i);
			
			// Save the value of the current i, otherwise it'll overright when we swap the values
			GameObject temp = charList[i];
			
			// Swap the new and old values
			charList[i] = charList[rnd];
			charList[rnd] = temp;
		}
	}

    void IncreaseTurnsCounter()
    {
        turnsCounter ++;
        
        if (turnsCounter % charactersRuntimeSet.Items.Count == 0f)
        {
            cyclesCounter ++;
            SortQueueByDelay(charactersRuntimeSet.Items);
            turnsManagerEvents.NewCycle.RaiseEvent();
        }
    }

    // void AddCharacter(GameObject character)
    // {
    //     turnsManagerEvents.charList.Add(character.GetComponent<CharManager>());
    // }

    // void RemoveCharacter(GameObject character)
    // {
    //     turnsManagerEvents.charList.Remove(character.GetComponent<CharManager>());

    //     var manager = character.GetComponent<CharManager>();
    //     if(manager == turnsManagerEvents.currentChar)
    //     {
    //         NextCharacter();
    //     }
    // }

    void UpdateList()
    {
        if(!charactersRuntimeSet.Items.Contains(turnsManagerEvents.charTakingTurn.gameObject))
        {
            NextCharacter();
        }
    }

    void EndBattle()
    {
        foreach (var item in charactersRuntimeSet.Items)
        {
            var character = item.GetComponent<CharManager>();
            character.StopListening();
            character.DisableComponents();
        }

        this.gameObject.SetActive(false);
    }
}
