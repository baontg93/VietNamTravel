public class BaseEvent : Enumeration
{
    private static int value = 0;
    protected BaseEvent(string displayName) : base(++value, displayName) { }
}
public class GameEvent : BaseEvent
{
    protected GameEvent(string displayName) : base(displayName) { }

    public static readonly GameEvent Connected = new("Connected");
    public static readonly GameEvent RequestError = new("RequestError");
    public static readonly GameEvent LandDataUpdated = new("LandDataUpdated");
    public static readonly GameEvent HumanDataUpdated = new("HumanDataUpdated");
    public static readonly GameEvent RefreshFarmScreen = new("RefreshFarmScreen");
    public static readonly GameEvent VisitMarket = new("VisitMarket");
}