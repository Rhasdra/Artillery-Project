using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnsManager : MonoBehaviour
{
    public static CharManager currentChar;
    [SerializeField] int index = 0;

    [SerializeField] CharManager[] charControllers;

    public static UnityEvent NextTurn = new UnityEvent();
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
        currentChar = charControllers[index];
        StartTurnFunc();
    }
    
    public void NextCharacter()
    {
        currentChar?.EndTurn.Invoke();

        if ( index < charControllers.Length -1)
        {
            index++;
            // currentChar = charControllers[index];
            // currentChar?.StartTurn.Invoke();
            // return;
        } else
        index = 0;

        currentChar = charControllers[index];
        StartTurnFunc();
    }

    void StartTurnFunc()
    {
        currentChar?.StartTurn.Invoke();
        StartTurn.Invoke();
    }
}
