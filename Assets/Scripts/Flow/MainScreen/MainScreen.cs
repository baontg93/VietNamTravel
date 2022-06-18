using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private UserInfo userInfo;
    [SerializeField] private TutorialScreen tutorialScreen;
    [SerializeField] private FriendListScreen friendListScreen;
    [SerializeField] private MapScreen mapScreen;
    [SerializeField] private AccountScreen userCollectDataScreen;
    [SerializeField] private CongratScreen congratScreen;

    private List<string> unlockedProvines = new();

    void Start()
    {
        MobileCloudServices.OnJoinGame += MobileCloudServices_OnJoinGame;
        tutorialScreen.OnHiden += TutorialScreen_OnHiden;
        userCollectDataScreen.OnSubmit += UserCollectDataScreen_OnSubmit;
        mapScreen.OnProvinceUnlocked += MapScreen_OnProvinceUnlocked;
        mapScreen.OnShown += MapScreen_OnShown;
        mapScreen.OnHiden += MapScreen_OnHiden;
    }

    private void MobileCloudServices_OnJoinGame(JoinGameData obj)
    {
        unlockedProvines = obj.UnlockedData.Provinces;

        if (!tutorialScreen.CheckCacheAndOpen())
        {
            if (!userCollectDataScreen.CheckCacheAndOpen())
            {
                UnlockFirstProvince();
            }
        }
    }

    private void TutorialScreen_OnHiden()
    {
        tutorialScreen.OnHiden -= TutorialScreen_OnHiden;
        if (!userCollectDataScreen.CheckCacheAndOpen())
        {
            UnlockFirstProvince();
        }
    }

    private void UnlockFirstProvince()
    {
        if (unlockedProvines == null || unlockedProvines.Count == 0)
        {
            mapScreen.Show();
            mapScreen.UnlockFirstProvince();
        }
    }

    private void UserCollectDataScreen_OnSubmit(string name)
    {
        userInfo.UpdateName(name);
        if (unlockedProvines == null || unlockedProvines.Count == 0)
        {
            mapScreen.UnlockFirstProvince();
        }
    }

    private void MapScreen_OnShown()
    {
        gameObject.SetActive(false);
    }

    private void MapScreen_OnHiden()
    {
        gameObject.SetActive(true);
    }

    private void MapScreen_OnProvinceUnlocked(string provine)
    {
        OpenMap();
        congratScreen.SetProvinceName(provine);
        congratScreen.Show();
        unlockedProvines.Add(provine);
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OpenMap(bool showChecking = false)
    {
        mapScreen.Show();
        if (showChecking)
        {
            mapScreen.ShowChecking();
        }
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