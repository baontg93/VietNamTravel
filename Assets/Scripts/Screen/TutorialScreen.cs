using UnityEngine;

public class TutorialScreen : BaseScreen
{
    public bool CheckCacheAndOpen()
    {
        bool isFinished = PlayerPrefs.GetString("TutorialScreen_Finished", "false") == "true";
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
        PlayerPrefs.SetString("TutorialScreen_Finished", "true");
        base.Hide();
    }
}