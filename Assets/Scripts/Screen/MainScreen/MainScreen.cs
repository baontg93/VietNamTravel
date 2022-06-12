using System;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    public UserInfo userInfo;
    public TutorialScreen tutorialScreen;
    public FriendListScreen friendListScreen;
    public MapScreen mapScreen;
    public UserCollectDataScreen userCollectDataScreen;

    void Start()
    {
        UserData userData = new();
        userData.Name = PlayerPrefs.GetString("MainScreen_name", "Unknown");
        userData.Avatar = "avatar_1";
        userInfo.UpdateData(userData);
        tutorialScreen.OnHiden += CheckAndOpenUserData;
        userCollectDataScreen.OnSubmit += OnNameSubmited;

        if (!tutorialScreen.CheckCacheAndOpen())
        {
            userCollectDataScreen.CheckCacheAndOpen();
        }
    }

    private void OnNameSubmited(string name)
    {
        PlayerPrefs.SetString("MainScreen_name", name);
        userInfo.UpdateName(name);
    }

    private void CheckAndOpenUserData()
    {
        tutorialScreen.OnHiden -= CheckAndOpenUserData;
        userCollectDataScreen.CheckCacheAndOpen();
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OpenMap()
    {
        mapScreen.Show();
    }

    public void CloseMap()
    {
        mapScreen.Hide();
    }

    public void OpenFriendList()
    {
        friendListScreen.Show();
    }

    public void CloseFriendList()
    {
        friendListScreen.Hide();
    }
}