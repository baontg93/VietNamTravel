public class StateTransition
{
    private readonly BaseState srcState;
    private readonly BaseState desState;
    private readonly BaseEvent baseEvent;
    private readonly IStateMachine stateMachine;

    public StateTransition(IStateMachine stateMachine, BaseState srcState, BaseState desState, BaseEvent baseEvent)
    {
        this.stateMachine = stateMachine;
        this.srcState = srcState;
        this.desState = desState;
        this.baseEvent = baseEvent;
    }

    public bool CanTransition(BaseEvent baseEvent)
    {
        if (baseEvent.Equals(this.baseEvent))
        {
            if (stateMachine.GetCurrentState() == srcState)
            {
                return true;
            }
        }
        return false;
    }

    public void Execute(object data)
    {
        stateMachine.ChangeState(desState, data);
    }
}
