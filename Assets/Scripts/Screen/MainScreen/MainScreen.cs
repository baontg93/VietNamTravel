using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private UserInfo userInfo;
    [SerializeField] private TutorialScreen tutorialScreen;
    [SerializeField] private FriendListScreen friendListScreen;
    [SerializeField] private MapScreen mapScreen;
    [SerializeField] private UserCollectDataScreen userCollectDataScreen;
    [SerializeField] private CongratScreen congratScreen;

    private List<string> unlockedProvines = new();

    void Start()
    {
        string json = PlayerPrefs.GetString("MainScreen_unlockedProvines");
        if (!string.IsNullOrEmpty(json))
        {
            unlockedProvines = JsonConvert.DeserializeObject<List<string>>(json);
        }
        UserData userData = new();
        userData.Name = PlayerPrefs.GetString("MainScreen_name", "Unknown");
        userData.Avatar = "avatar_1";
        userInfo.UpdateData(userData);
        tutorialScreen.OnHiden += OnTutorialHiden;
        userCollectDataScreen.OnSubmit += OnNameSubmited;
        mapScreen.OnProvinceUnlocked += OnProvinceUnlocked;

        if (!tutorialScreen.CheckCacheAndOpen())
        {
            userCollectDataScreen.CheckCacheAndOpen();
        }
    }

    private void OnNameSubmited(string name)
    {
        PlayerPrefs.SetString("MainScreen_name", name);
        userInfo.UpdateName(name);
        if (unlockedProvines.Count == 0)
        {
            mapScreen.UnlockFirstProvince();
        }
    }

    private void OnTutorialHiden()
    {
        tutorialScreen.OnHiden -= OnTutorialHiden;
        userCollectDataScreen.CheckCacheAndOpen();
    }

    public void OnProvinceUnlocked(string provine)
    {
        congratScreen.SetProvinceName(provine);
        congratScreen.Show();

        unlockedProvines.Add(provine);
        string json = JsonConvert.SerializeObject(unlockedProvines);
        PlayerPrefs.SetString("MainScreen_unlockedProvines", json);
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