using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapScreen : BaseScreen
{
    public TextMeshProUGUI AddressText;

    public event Action<string> OnProvinceUnlocked = delegate { };

    public LocationManager locationManager;

    public UnlockedData UnlockedData = new();

    public GameObject Checking;
    public Button BtnChecking;
    public Button BtnResetCam;
    public VietNamMap CheckingMap;
    public GameObject HomeMap;

    public override void Start()
    {
        base.Start();

        ProvincesParser.Provinces = new();
        foreach (var item in ProvincesParser.DataProvinces)
        {
            ProvincesParser.Provinces.Add(item.Value);
        }

        locationManager.OnLocation_Updated += OnLocation_Updated;
        MobileCloudServices.OnDataReceived += MobileCloudServices_OnDataReceived;
        MobileCloudServices.OnJoinGame += MobileCloudServices_OnJoinGame;
    }

    public void FocusOn(string province)
    {
        BtnResetCam.gameObject.SetActive(true);
        AddressText.text = province;
        CheckingMap.FocusOn(province);
        HideChecking();
    }

    public void ResetCam(bool playAnim = false)
    {
        CheckingMap.ResetCam(playAnim);
        BtnResetCam.gameObject.SetActive(false);
        AddressText.text = "";
    }

    private void MobileCloudServices_OnJoinGame(JoinGameData obj)
    {
        UnlockedData = obj.UnlockedData;
    }

    private void MobileCloudServices_OnDataReceived(string key, object data)
    {
        if (key == StogrageKey.USER_UNLOCKED_DATA)
        {
            UnlockedData = (UnlockedData)data;
        }
    }

    private void OnLocation_Updated(string adrress, string province)
    {
    }

    public void ShowChecking()
    {
        Checking.SetActive(true);
        BtnChecking.interactable = false;
    }

    public void HideChecking()
    {
        Checking.SetActive(false);
        BtnChecking.interactable = true;
    }

    public override void Show()
    {
        base.Show();
        HomeMap.SetActive(false);
        CheckingMap.gameObject.SetActive(true);
        locationManager.CheckStatus();
    }

    public override void Hide()
    {
        base.Hide();
        HomeMap.SetActive(true);
        CheckingMap.gameObject.SetActive(false);
        HideChecking();
        ResetCam();
        AddressText.text = "";
    }

    public void UnlockFirstProvince()
    {
        string province = locationManager.Province;
        UnlockProvince(province);
    }

    public void UnlockProvince(string province)
    {
        if (string.IsNullOrEmpty(province))
        {
            return;
        }
        UnlockedData.Provinces.AddNonExistItem(province);
        UnlockedData.Provinces.Sort();
        MobileStorage.SetObject(StogrageKey.USER_UNLOCKED_DATA, UnlockedData);
        FocusOn(province);
        OnProvinceUnlocked(province);
    }
}
