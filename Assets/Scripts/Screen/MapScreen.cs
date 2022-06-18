using System;
using TMPro;
using UnityEngine;

public class MapScreen : BaseScreen
{
    public GameObject Map;

    public TextMeshProUGUI AddressText;
    public TextMeshProUGUI ProvinceText;

    public event Action<string> OnProvinceUnlocked = delegate { };

    public LocationManager locationManager;

    public UnlockedData UnlockedData = new();

    public override void Start()
    {
        base.Start();
        locationManager.OnLocation_Updated += OnLocation_Updated;
        MobileCloudServices.OnDataReceived += MobileCloudServices_OnDataReceived;
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
        ProvinceText.text = province;
    }

    public override void Show()
    {
        base.Show();
        locationManager.CheckStatus();
    }


    public void UnlockFirstProvince()
    {
        string province = locationManager.Address;
        UnlockProvince(province);
    }

    public void UnlockProvince(string province)
    {
        UnlockedData.Provinces.Add(province);
        MobileStorage.SetObject(StogrageKey.USER_UNLOCKED_DATA, UnlockedData);

        OnProvinceUnlocked(province);
    }
}
