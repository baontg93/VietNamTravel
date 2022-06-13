using System;
using TMPro;
using UnityEngine;

public class MapScreen : BaseScreen
{
    public GameObject Map;

    public TextMeshProUGUI AddressText;
    public TextMeshProUGUI ProvinceText;

    public Action<string> OnProvinceUnlocked = delegate { };

    public LocationManager locationManager;

    public override void Start()
    {
        base.Start();
        locationManager.OnLocation_Updated += OnLocation_Updated;
    }

    private void OnLocation_Updated(string adrress, string province)
    {
        AddressText.text = adrress;
        ProvinceText.text = province;
    }

    public override void Show()
    {
        base.Show();
        Map.SetActive(true);
        locationManager.CheckStatus();
    }

    public override void Hide()
    {
        base.Hide();
        Map.SetActive(false);
    }

    public override void Reset()
    {
        base.Reset();
        Map.SetActive(false);
    }

    public void UnlockFirstProvince()
    {
        string province = locationManager.Address;
        OnProvinceUnlocked(province);
    }
}
