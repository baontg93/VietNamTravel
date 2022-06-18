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
        MobileCloudServices.OnJoinGame += OnJoinGame;
        tutorialScreen.OnHiden += OnTutorialHiden;
        userCollectDataScreen.OnSubmit += OnNameSubmited;
        mapScreen.OnProvinceUnlocked += OnProvinceUnlocked;
    }

    private void OnJoinGame(JoinGameData obj)
    {
        unlockedProvines = obj.UnlockedData.Provinces;

        if (!tutorialScreen.CheckCacheAndOpen())
        {
            userCollectDataScreen.CheckCacheAndOpen();
        }
    }

    private void OnNameSubmited(string name)
    {
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
        OpenMap();
        congratScreen.SetProvinceName(provine);
        congratScreen.Show();
        unlockedProvines.Add(provine);
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