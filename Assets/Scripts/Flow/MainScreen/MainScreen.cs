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

        EventManager.Instance.Register(GameEvent.FocusOnProvince, OnFocusOnProvince);
        EventManager.Instance.Register(GameEvent.DoUnlockProvince, OnDoUnlockProvince);
    }

    private void OnFocusOnProvince(object province)
    {
        mapScreen.FocusOn((string)province);
    }

    private void OnDoUnlockProvince(object province)
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
            mapScreen.UnlockFirstProvince();
        }
    }

    private void UserCollectDataScreen_OnSubmit(string name, Sprite avatar)
    {
        userInfo.UpdateName(name);
        userInfo.UpdateAvatar(avatar);
        UnlockFirstProvince();
    }

    private void MapScreen_OnProvinceUnlocked(string provine)
    {
        congratScreen.Show(provine);
        congratScreen.Show();
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
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