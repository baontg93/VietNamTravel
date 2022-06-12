using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public string name;

    protected BaseStateMachine stateMachine;

    public BaseState(string name, BaseStateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}