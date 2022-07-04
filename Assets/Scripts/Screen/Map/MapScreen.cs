using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapScreen : MonoBehaviour
{
    public ScriptableProvince ScriptableProvince;
    public TextAsset JsonFile;
    public ScrollAndPinch MapScroll;
    public ProvinceInfo ProvinceInfo;
    public event Action<string> OnProvinceUnlocked = delegate { };

    public LocationManager locationManager;

    public UnlockedData UnlockedData = new();

    public BaseScreen Checking;
    public Button BtnChecking;
    public Button BtnResetCam;
    public VietNamMap CheckingMap;
    public MapPointerSelect MapPointerSelect;

    public void Awake()
    {
        ProvincesParser.Provinces = new();
        foreach (var item in ProvincesParser.DataProvinces)
        {
            ProvincesParser.Provinces.Add(item.Value);
        }

        CheckingMap.OnDataSetted += CheckingMap_OnDataSetted;
        locationManager.OnLocation_Updated += OnLocation_Updated;
        MapScroll.OnScrollMap += MapScroll_OnScrollMap;
        MapPointerSelect.OnSelected += MapPointerSelect_OnSelected;
        MobileCloudServices.OnDataReceived += MobileCloudServices_OnDataReceived;
        MobileCloudServices.OnJoinGame += MobileCloudServices_OnJoinGame;
        ResetCam(false);
    }

    private void CheckingMap_OnDataSetted(Dictionary<string, Transform> dictProvinces)
    {
        ScriptableProvince.Init(JsonFile.text, dictProvinces);
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
        ProvinceInfo.ShowData(province, UnlockedData.GetUnlockTime(province));
        CheckingMap.FocusOn(province);
        HideChecking();
    }

    public void ResetCam(bool playAnim = false)
    {
        CheckingMap.ResetCam(playAnim);
        BtnResetCam.gameObject.SetActive(false);
        ProvinceInfo.Clean();
    }

    private void MobileCloudServices_OnJoinGame(JoinGameData obj)
    {
        UnlockedData = obj.UnlockedData;
    }

    public void GameReady()
    {
        locationManager.CheckStatus();
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

    public void UnlockProvince(string province)
    {
        if (string.IsNullOrEmpty(province))
        {
            return;
        }
        UnlockedData.Unlock(province);
        MobileStorage.SetObject(StogrageKey.USER_UNLOCKED_DATA, UnlockedData);
        FocusOn(province);
        OnProvinceUnlocked(province);

        MobileGameServices.Instance.SetScore(UnlockedData.Provinces.Count);
    }
}
