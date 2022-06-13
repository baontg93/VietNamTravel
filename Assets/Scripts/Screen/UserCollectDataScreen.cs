using System;
using TMPro;
using UnityEngine;
using VoxelBusters.EssentialKit;

public class UserCollectDataScreen : BaseScreen
{
    public PushNotification pushNotification; 
    public TMP_InputField InputName;
    public TMP_InputField InputMail;
    public TMP_InputField InputMessage;

    public Action<string> OnSubmit = delegate { };

    public void Submit()
    {
        string name = InputName.text;
        string mail = InputMail.text;
        string msg = InputMessage.text;

        Debug.Log("Name = " + name + "\nMail = " + mail + "\nMessage = " + msg);
        OnSubmit?.Invoke(name);
        Hide();
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
        pushNotification.CheckPermission();
    }

    public override void Hide()
    {
        PlayerPrefs.SetString("UserCollectDataScreen_Finished", "true");
        base.Hide();
    }
}
