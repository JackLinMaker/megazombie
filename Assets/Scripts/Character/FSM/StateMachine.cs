using UnityEngine;
using System.Collections;

public class StateMachine<T> {
    private T owner;
    private State<T> currentState;
    private State<T> previousState;
    private State<T> globalState;

    public StateMachine(T Owner)
    {
        owner = Owner;
        currentState = null;
        previousState = null;
        globalState = null;
    }

    public void SetCurrentState(State<T> s)
    {
        currentState = s;
    }

    public State<T> CurrentState()
    {
        return currentState;
    }

    public void SetGlobalState(State<T> s)
    {
        globalState = s;
    }

    public State<T> GlobalState()
    {
        return globalState;
    }

    public void SetPreviousState(State<T> s)
    {
        previousState = s;
    }

    public State<T> PreviousState()
    {
        return previousState;
    }


    public void Update()
    {
        if (globalState != null)
            globalState.Execute(owner);
        if (currentState != null)
            currentState.Execute(owner);
    }

    public void ChangeState(State<T> newState)
    {
        previousState = currentState;
        if (currentState != null)
        {
            currentState.Exit(owner);
        }

        currentState = newState;
        if (currentState != null)
        {
            currentState.Enter(owner);
        }
    }

    public void RevertToPreviousState()
    {
        if (previousState != null)
            ChangeState(previousState);
    }

    public bool IsInState(State<T> s)
    {
        if (s == currentState)
            return true;
        return false;
    }

    

   

}
