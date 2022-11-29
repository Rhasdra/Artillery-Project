using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class TurnsManager : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public static CharManager currentChar;
    public float turnsCounter = 0;
     public float macroTurnsCounter = 0;
    [SerializeField] int index = 0;

    public List<CharManager> charManagers;

    public static UnityEvent NextTurn = new UnityEvent();
    public static UnityEvent NextMacroTurn = new UnityEvent();
    public static UnityEvent StartTurn = new UnityEvent();

    private void OnEnable() 
    {
        NextTurn.AddListener(NextCharacter);
    }

    private void OnDisable() 
    {
        NextTurn.RemoveListener(NextCharacter);
    }

    private void Start() 
    {
        currentChar = charManagers[index];
        StartTurnFunc();
    }
    
    public void NextCharacter()
    {
        currentChar?.EndTurn.Invoke();

        if ( index < charManagers.Count -1)
        {
            index++;
            // currentChar = charControllers[index];
            // currentChar?.StartTurn.Invoke();
            // return;
        } else {
        index = 0;
        }

        IncreaseTurnsCounter();
        currentChar = charManagers[index];

        StartTurnFunc();
    }

    void StartTurnFunc()
    {
        currentChar?.StartTurn.Invoke();
        
        if(cam != null)
        {
            cam.Follow = currentChar.transform;
        }
        
        StartTurn.Invoke();
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
                if (unsorted[j].gameObject.GetComponent<Delay>().delay < unsorted[min].gameObject.GetComponent<Delay>().delay)
                {
                    min = j;
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

	void Shuffle(List<CharManager> a)
	{
		// Loops through array
		for (int i = a.Count-1; i > 0; i--)
		{
			// Randomize a number between 0 and i (so that the range decreases each time)
			int rnd = Random.Range(0,i);
			
			// Save the value of the current i, otherwise it'll overright when we swap the values
			CharManager temp = a[i];
			
			// Swap the new and old values
			a[i] = a[rnd];
			a[rnd] = temp;
		}
	}

    void IncreaseTurnsCounter()
    {
        turnsCounter ++;
        
        if (turnsCounter % charManagers.Count == 0f)
        {
            macroTurnsCounter ++;
            SortQueueByDelay(charManagers);
            NextMacroTurn.Invoke();
            //NextMacroTurnEvent();
        }
    }
}
