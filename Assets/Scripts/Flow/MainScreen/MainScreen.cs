using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private UserInfo userInfo;
    [SerializeField] private TutorialScreen tutorialScreen;
    [SerializeField] private FriendListScreen friendListScreen;
    [SerializeField] private MapScreen mapScreen;
    [SerializeField] private AccountScreen userCollectDataScreen;
    [SerializeField] private CongratScreen congratScreen;

    void Start()
    {
        MobileCloudServices.OnJoinGame += MobileCloudServices_OnJoinGame;
        tutorialScreen.OnHiden += TutorialScreen_OnHiden;
        userCollectDataScreen.OnSubmit += UserCollectDataScreen_OnSubmit;
        mapScreen.OnProvinceUnlocked += MapScreen_OnProvinceUnlocked;
        mapScreen.OnShown += MapScreen_OnShown;
        mapScreen.OnHiden += MapScreen_OnHiden;

        EventManager.Instance.Register(GameEvent.FocusOnProvince, OnFocusOnProvince);
    }

    private void OnFocusOnProvince(object province)
    {
        mapScreen.UnlockProvince((string)province);
    }

    private void MobileCloudServices_OnJoinGame(JoinGameData obj)
    {
        Debug.Log("On Join Game");
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
        if (mapScreen.UnlockedData.Provinces.Count == 0)
        {
            mapScreen.Show();
            mapScreen.UnlockFirstProvince();
        }
    }

    private void UserCollectDataScreen_OnSubmit(string name, Sprite avatar)
    {
        userInfo.UpdateName(name);
        userInfo.UpdateAvatar(avatar);
        UnlockFirstProvince();
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
        congratScreen.Show(provine);
        congratScreen.Show();
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