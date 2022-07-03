using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapScreen : MonoBehaviour
{
    public TextMeshProUGUI AddressText;
    public ScrollAndPinch MapScroll;
    public event Action<string> OnProvinceUnlocked = delegate { };

    public LocationManager locationManager;

    public UnlockedData UnlockedData = new();

    public BaseScreen Checking;
    public Button BtnChecking;
    public Button BtnResetCam;
    public VietNamMap CheckingMap;
    public MapPointerSelect MapPointerSelect;

    public void Start()
    {
        ProvincesParser.Provinces = new();
        foreach (var item in ProvincesParser.DataProvinces)
        {
            ProvincesParser.Provinces.Add(item.Value);
        }

        locationManager.OnLocation_Updated += OnLocation_Updated;
        MapScroll.OnScrollMap += MapScroll_OnScrollMap;
        MapPointerSelect.OnSelected += MapPointerSelect_OnSelected;
        MobileCloudServices.OnDataReceived += MobileCloudServices_OnDataReceived;
        MobileCloudServices.OnJoinGame += MobileCloudServices_OnJoinGame;
        ResetCam(false);
    }

    private void MapPointerSelect_OnSelected(string province)
    {
        if (!string.IsNullOrEmpty(province))
        {
            FocusOn(province);
        }
    }

    private void MapScroll_OnScrollMap()
    {
        BtnResetCam.gameObject.SetActive(true);
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
        Checking.Show();
        BtnChecking.interactable = false;
    }

    public void HideChecking()
    {
        Checking.Hide();
        BtnChecking.interactable = true;
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
