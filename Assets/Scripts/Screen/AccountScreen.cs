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


    public event Action<string, Sprite> OnSubmit;

    public override void Start()
    {
        base.Start();
        EmptyAchivement.SetActive(true);
        ContentAchivement.SetActive(false);
        InputName.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void LoadAchievements()
    {
        MobileGameServices.Instance.LoadAchievements((datas) =>
        {
            for (int i = ContentAchivement.transform.childCount - 1; i >= 0; i--)
            {
                EZ_Pooling.EZ_PoolManager.Despawn(ContentAchivement.transform.GetChild(i));
            }
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
                    item.UpdateData(data);
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

        InactiveScreen.Instance.Hide();
    }

    public void Submit()
    {
        string name = InputName.text;

        if (isShown)
        {
            MobileStorage.SetBool(StogrageKey.ACCOUNT_SETTING_FINISH, true);
        }

        OnSubmit?.Invoke(name, Avatar.sprite);
        Hide();
    }

    public void SelectAvatar()
    {
        AvatarLoader.Instance.SelectAvatar((sprite) =>
        {
            if (sprite != null)
            {
                Avatar.sprite = sprite;
            }
        });
    }

    public bool CheckCacheAndOpen()
    {
        bool isFinished = MobileStorage.GetBool(StogrageKey.ACCOUNT_SETTING_FINISH);
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

        if (MobileGameServices.Instance.IsReady())
        {
            LoadAchievements();
        }
    }

    public override void Hide()
    {
        ButtonClose.gameObject.SetActive(true);
        base.Hide();
    }
}
