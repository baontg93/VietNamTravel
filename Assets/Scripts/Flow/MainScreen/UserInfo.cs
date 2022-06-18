using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : SingletonBehaviour<UserInfo>
{
    public Image Avatar;
    public TextMeshProUGUI Name;

    private void Start()
    {
        MobileCloudServices.OnJoinGame += MobileCloudServices_OnJoinGame;
        MobileCloudServices.OnDataReceived += MobileCloudServices_OnDataReceived;
    }

    private void MobileCloudServices_OnJoinGame(JoinGameData obj)
    {
        Name.text = obj.Username;
        if (obj.Avatar)
        {
            Avatar.sprite = obj.Avatar;
        }
        else
        {
            Avatar.sprite = AvatarLoader.Instance.GetAvatar(Random.Range(1, 32));
        }
    }

    private void MobileCloudServices_OnDataReceived(string key, object data)
    {
        switch (key)
        {
            case StogrageKey.USER_NAME:
                Name.text = (string)data;
                break;

            case StogrageKey.USER_AVATAR:
                Avatar.sprite = (Sprite)data;
                break;
        }
    }

    public void UpdateName(string name)
    {
        Name.text = name;

        MobileStorage.SetString(StogrageKey.USER_NAME, name);
    }

    public void UpdateAvatar(Sprite avatar)
    {
        Avatar.sprite = avatar;

        MobileStorage.SetSprite(StogrageKey.USER_AVATAR, avatar);
    }
}
