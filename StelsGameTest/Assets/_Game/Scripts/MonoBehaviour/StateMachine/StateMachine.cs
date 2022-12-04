using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateMachine : MonoBehaviour
{
    public Dictionary<Type, IState> States { get; set; }
    public IState CurrentState { get; private set; }

    protected virtual void Awake()
    {
        States = new Dictionary<Type, IState>();
        InitializeStates();
    }

    private void Update()
    {
        if (CurrentState != null) CurrentState.Update();
    }

    protected abstract void InitializeStates();

    protected void SetState(IState newState)
    {
        if (CurrentState != null) CurrentState.Exit();

        CurrentState = newState;
        CurrentState.Enter();
    }

    public void ResetState()
    {
        if (CurrentState != null) CurrentState.Exit();
        CurrentState = null;
    }

    protected IState GetState<T>() where T : IState
    {
        var type = typeof(T);
        return States[type];
    }
}