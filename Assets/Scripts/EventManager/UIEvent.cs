public class UIEvent : BaseEvent
{
    protected UIEvent(string displayName) : base(displayName) { }

    public static readonly UIEvent OpenMap = new("OpenMap");
    public static readonly UIEvent CloseMap = new("CloseMap");

    public static readonly UIEvent OpenFriendList = new("OpenFriendList");
    public static readonly UIEvent CloseFriendList = new("CloseFriendList");

    public static readonly UIEvent OpenTutorial = new("OpenTutorial");
    public static readonly UIEvent CloseTutorial = new("CloseTutorial");
}