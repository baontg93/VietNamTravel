using System.Collections;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private UserInfo userInfo;
    [SerializeField] private TutorialScreen tutorialScreen;
    [SerializeField] private FriendListScreen friendListScreen;
    [SerializeField] private MapScreen mapScreen;
    [SerializeField] private AccountScreen userCollectDataScreen;
    [SerializeField] private CongratScreen congratScreen;

    private bool isReady = false;

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
        StartCoroutine(CheckAndUnlockProvince((string)province));
    }

    private void MobileCloudServices_OnJoinGame(JoinGameData obj)
    {
        Debug.Log("On Join Game");
        if (!tutorialScreen.CheckCacheAndOpen())
        {
            TutorialScreen_OnHiden();
        }
    }

    private void TutorialScreen_OnHiden()
    {
        tutorialScreen.OnHiden -= TutorialScreen_OnHiden;
        if (!userCollectDataScreen.CheckCacheAndOpen())
        {
            GameReady();
        }
    }

    private IEnumerator CheckAndUnlockProvince(string province)
    {
        yield return new WaitUntil(() => isReady);
        if (!mapScreen.UnlockedData.IsUnlocked(province))
        {
            mapScreen.UnlockProvince(province);
        }
    }

    private void UserCollectDataScreen_OnSubmit(string name, Sprite avatar)
    {
        GameReady();
        userInfo.UpdateName(name);
        userInfo.UpdateAvatar(avatar);
    }

    private void GameReady()
    {
        isReady = true;
        mapScreen.GameReady();
    }

    private void MapScreen_OnProvinceUnlocked(string provine)
    {
        congratScreen.Show(provine);
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