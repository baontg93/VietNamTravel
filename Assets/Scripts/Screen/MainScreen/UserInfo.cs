using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{
    public Image Avatar;
    public TextMeshProUGUI Name;

    public void UpdateName(string name)
    {
        Name.text = name;
    }
    public void UpdateData(UserData userData)
    {
        Name.text = userData.Name;
        Avatar.sprite = AvatarLoader.Instance.GetAvatar(userData.Avatar);
    }
}
