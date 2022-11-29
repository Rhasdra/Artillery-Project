using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	public IState currentState { get; private set; }
	public IState _previousState;

    public bool debug;

	bool _inTransition = false;

	public void ChangeState(IState newState)
	{
		// ensure we're ready for a new state
		if (currentState == newState || _inTransition)
			return;

		ChangeStateRoutine(newState);
	}

	public void RevertState()
	{
		if (_previousState != null)
			ChangeState(_previousState);
	}

	void ChangeStateRoutine(IState newState)
	{
		_inTransition = true;
		// begin our exit sequence, to prepare for new state
		if (currentState != null)
			{
                currentState.Exit();

                if (debug)
                Debug.Log(gameObject.name.ToString() + " " + currentState + " Exit!");
            }

		// save our current state, in case we want to return to it
		if (_previousState != null)
			_previousState = currentState;

		currentState = newState;

		// begin our new Enter sequence
		if (currentState != null)
        {
			
            currentState.Enter();

                if (debug)
                Debug.Log(gameObject.name.ToString() + " " + currentState + " Enter!");

        }

		_inTransition = false;
	}

	// pass down Update ticks to States, since they won't have a MonoBehaviour
	public void Update()
	{
		// simulate update ticks in states
		if (currentState != null && !_inTransition)
			currentState.Tick();
	}

    public void FixedUpdate()
    {
		// simulate fixedUpdate ticks in states
		if (currentState != null && !_inTransition)
			currentState.FixedTick();
    }

    public void LateUpdate()
    {
		// simulate fixedUpdate ticks in states
		if (currentState != null && !_inTransition)
			currentState.LateTick();
    }
}
