public class TutorialScreen : BaseScreen
{
    public bool CheckCacheAndOpen()
    {
        bool isFinished = MobileStorage.GetBool(StogrageKey.TUTORIAL_FINISH);
        if (!isFinished)
        {
            Show();
        }
        else
        {
            Hide();
            return false;
        }

        return true;
    }

    public override void Hide()
    {
        MobileStorage.SetBool(StogrageKey.TUTORIAL_FINISH, true);
        base.Hide();
    }
}