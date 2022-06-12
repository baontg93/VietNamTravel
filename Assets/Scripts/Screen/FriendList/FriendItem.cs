using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    public Image Avatar;
    public TextMeshProUGUI Name;

    public void UpdateData(UserData friendData)
    {
        Name.text = friendData.Name;
        Avatar.sprite = AvatarLoader.Instance.GetAvatar(friendData.Avatar);
    }
}