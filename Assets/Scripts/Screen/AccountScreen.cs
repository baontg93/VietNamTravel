using System;
using TMPro;
using UnityEngine.UI;

public class AccountScreen : BaseScreen
{
    public PushNotification pushNotification; 
    public TMP_InputField InputName;
    public Image Avatar;
    public Button ButtonSubmit;

    public Action<string> OnSubmit = delegate { };

    public override void Start()
    {
        InputName.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnInputValueChanged(string value)
    {
        ButtonSubmit.interactable = value.Trim().Length >= 4;
    }

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
            if (sprite != null)
            {
                Avatar.sprite = sprite;
                UserInfo.Instance.UpdateAvatar(sprite);
            }
        });
    }

    public bool CheckCacheAndOpen()
    {
        bool isFinished = MobileStorage.GetBool(StogrageKey.ACCOUTN_SETTING_FINISH);
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
        InputName.SetTextWithoutNotify(UserInfo.Instance.Name.text);
        Avatar.sprite = UserInfo.Instance.Avatar.sprite;
        pushNotification.CheckPermission();
    }

    public override void Hide()
    {
        MobileStorage.SetBool(StogrageKey.ACCOUTN_SETTING_FINISH, true);
        base.Hide();
    }
}
