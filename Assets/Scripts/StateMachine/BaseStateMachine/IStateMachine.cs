public interface IStateMachine
{
    public void ChangeState(BaseState newState, object data);
    public BaseState GetCurrentState();
}
