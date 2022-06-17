using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountScreen : BaseScreen
{
    public PushNotification pushNotification; 
    public TMP_InputField InputName;
    public Image Avatar;

    public Action<string> OnSubmit = delegate { };

    public void Submit()
    {
        string name = InputName.text;

        OnSubmit?.Invoke(name);
        Hide();
    }

    public void SelectAvatar()
    {
        AvatarLoader.Instance.SelectAvatar((sprite) =>
        {
            Avatar.sprite = sprite;
            UserInfo.Instance.Avatar.sprite = sprite;
        });
    }

    public bool CheckCacheAndOpen()
    {
        bool isFinished = PlayerPrefs.GetString("UserCollectDataScreen_Finished", "false") == "true";
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

    public override void Show()
    {
        base.Show();
        Avatar.sprite = UserInfo.Instance.Avatar.sprite;
        pushNotification.CheckPermission();
    }

    public override void Hide()
    {
        PlayerPrefs.SetString("UserCollectDataScreen_Finished", "true");
        base.Hide();
    }
}
