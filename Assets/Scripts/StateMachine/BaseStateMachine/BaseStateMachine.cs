using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour, IStateMachine
{
    BaseState currentState;
    readonly List<StateTransition> stateTransitions = new();
    readonly List<BaseEvent> events = new();

    private void Start()
    {
        Register();
        StartLifeCircle();
    }

    protected abstract void Register();
    protected abstract BaseState GetInitialState();

    protected void StartLifeCircle()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter(null);

        for (int i = 0; i < events.Count; i++)
        {
            BaseEvent baseEvent = events[i];
            EventManager.Instance.Register(baseEvent, (data) =>
            {
                CheckAndTransit(baseEvent, data);
            });
        }
    }

    protected void CheckAndTransit(BaseEvent baseEvent, object data)
    {
        for (int i = 0; i < stateTransitions.Count; i++)
        {
            StateTransition stateTransition = stateTransitions[i];
            if (stateTransition.CanTransition(baseEvent))
            {
                stateTransition.Execute(data);
                break;
            }
        }
    }

    protected void AddTransition(BaseState srcState, BaseState desState, BaseEvent baseEvent)
    {
        StateTransition stateTransition = new(this, srcState, desState, baseEvent);
        stateTransitions.Add(stateTransition);
        if (!events.Contains(baseEvent))
        {
            events.Add(baseEvent);
        }
    }

    public void ChangeState(BaseState newState, object data)
    {
        currentState.Exit(data);
        currentState = newState;
        newState.Enter(data);
    }

    public BaseState GetCurrentState()
    {
        return currentState;
    }
}