public class BaseEvent : Enumeration
{
    private static int value = 0;
    protected BaseEvent(string displayName) : base(++value, displayName) { }
}
public class GameEvent : BaseEvent
{
    protected GameEvent(string displayName) : base(displayName) { }

    public static readonly GameEvent FocusOnProvince = new("FocusOnProvince");
    public static readonly GameEvent DoUnlockProvince = new("DoUnlockProvince");
}