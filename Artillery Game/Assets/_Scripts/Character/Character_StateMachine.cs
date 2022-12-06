using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_StateMachine : StateMachine
{
    CharManager charManager;

    State_WaitingForTurn waitingForTurn;
    State_TakingTurn takingTurn;
    State_Dead dead;

    private void Awake() 
    {
        charManager = GetComponent<CharManager>();

        waitingForTurn = new State_WaitingForTurn();
        takingTurn = new State_TakingTurn();
        dead = new State_Dead();
    }

    private void OnEnable() 
    {
        GetComponentInChildren<HealthPool>()?.CharacterDied.AddListener(Death);
    }

    private void Start() 
    {
        ChangeState(waitingForTurn);
    }

    void StartTurn()
    {
        ChangeState(takingTurn);
    }

    void EndTurn()
    {
        ChangeState(waitingForTurn);
    }

    void Death()
    {
        ChangeState(dead);
    }
}
