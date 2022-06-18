using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountScreen : BaseScreen
{
    public PushNotification pushNotification; 
    public TMP_InputField InputName;
    public Image Avatar;
    public Button ButtonSubmit;
    public Button ButtonClose;
    public GameObject EmptyAchivement;
    public GameObject ContentAchivement;
    public Transform PrefabAchivement;

    public event Action<string> OnSubmit;

    public override void Start()
    {
        base.Start();
        EmptyAchivement.SetActive(true);
        ContentAchivement.SetActive(false);
        InputName.onValueChanged.AddListener(OnInputValueChanged);
        MobileGameServices.OnAuthenticated += MobileGameServices_OnAuthenticated;
    }

    private void MobileGameServices_OnAuthenticated()
    {
        MobileGameServices.Instance.LoadAchievements((datas) =>
        {
            if (datas != null && datas.Count > 0)
            {
                EmptyAchivement.SetActive(false);
                ContentAchivement.SetActive(true);

                for (int i = 0; i < datas.Count; i++)
                {
                    var data = datas[i];
                    Transform tf = EZ_Pooling.EZ_PoolManager.Spawn(PrefabAchivement);
                    tf.SetParent(ContentAchivement.transform);
                    tf.localPosition = Vector3.zero;
                    tf.localScale = Vector3.one;

                    AchievementItem item = tf.GetComponent<AchievementItem>();
                }
            }
            else
            {
                EmptyAchivement.SetActive(true);
                ContentAchivement.SetActive(false);
            }
        });
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
            ButtonClose.gameObject.SetActive(false);
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
        ButtonClose.gameObject.SetActive(true);
        base.Hide();
    }
}
