using System;
using TMPro;
using UnityEngine;

public class MapScreen : BaseScreen
{
    public GameObject Map;

    public TextMeshProUGUI AddressText;

    public event Action<string> OnProvinceUnlocked = delegate { };

    public LocationManager locationManager;

    public UnlockedData UnlockedData = new();

    public GameObject Checking;

    public override void Start()
    {
        base.Start();
        locationManager.OnLocation_Updated += OnLocation_Updated;
        MobileCloudServices.OnDataReceived += MobileCloudServices_OnDataReceived;
        MobileCloudServices.OnJoinGame += MobileCloudServices_OnJoinGame;
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
        AddressText.text = adrress;
    }

    public void ShowChecking()
    {
        Checking.SetActive(true);
    }

    public override void Show()
    {
        base.Show();
        locationManager.CheckStatus();
    }

    public override void Reset()
    {
        base.Reset();
        Checking.SetActive(false);
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
        MobileStorage.SetObject(StogrageKey.USER_UNLOCKED_DATA, UnlockedData);

        OnProvinceUnlocked(province);
    }
}
