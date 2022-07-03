using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    string msgReceivingGameObjectName;
    string msgReceivingMethodName;

    string location;
    public string Province;
    public string Address;
    private bool ischecked = false;

    public Action<string, string> OnLocation_Updated = delegate { };

    void Start()
    {
        msgReceivingGameObjectName = "LocationManager";
        msgReceivingMethodName = "SetLocationCallbackMessage";
        SetCallback();
    }

    public void SetLocationCallbackMessage(string messageTemp)
    {
        Debug.Log("messageTemp " + messageTemp);
        string[] values = messageTemp.Split('/');
        if (values.Length > 1)
        {
            string finalMsg = "";
            if (values[0] == "Address")
            {
                for (int i = 1; i < values.Length; i++)
                {
                    if (values[i] != null && !values[i].Contains("null"))
                    {
                        finalMsg += values[i] + " - ";
                    }
                }
                Address = finalMsg;
                Province = ProvincesParser.GetProvince(finalMsg);
                if (!string.IsNullOrEmpty(Province))
                {
                    UpdateLocation();
                }
            }
            else if (values[0] == "Location")
            {
                for (int i = 1; i < values.Length; i++)
                {
                    finalMsg += values[i];
                }
                location = finalMsg;
#if !UNITY_EDITOR
                LocationManagerBridge.getAddressForCurrentLocation();
#endif
            }
        }

    }

    void UpdateLocation()
    {
        OnLocation_Updated?.Invoke(Address, Province);
        EventManager.Instance.Publish(GameEvent.DoUnlockProvince, Province);
    }

    void SetCallback()
    {
#if !UNITY_EDITOR
        LocationManagerBridge.setMessageReceivingObjectName(msgReceivingGameObjectName, msgReceivingMethodName);
#endif
    }

    public void CheckStatus()
    {
        ischecked = true;
#if !UNITY_EDITOR
        int temp = LocationManagerBridge.getAuthrizationLevelForApplication();
        Debug.Log("LocationManager's status = " + temp);
        switch (temp)
        {
            case 0: //"Not Determined"
                RequestPermission();
                break;
            case 1: //"Restricted"
            case 2: //"Denied"
                RequestSetting();
                break;
            case 3: //"Authorized Always"
            case 4: //"Authorized When In Use"
                LocationManagerBridge.startLocationMonitoring();
                break;
            default:
                Debug.Log("Location OK");
                break;
        }
#else
        Address = "Ho Chi Minh city";
        Province = ProvincesParser.GetProvince(Address);
        UpdateLocation();
#endif
    }

    private void RequestPermission()
    {
        LocationManagerBridge.requestAuthorizedWhenInUse();
    }

    private void RequestSetting()
    {
        LocationManagerBridge.showAlertForPermissions("Location services are off", "To use background location you must turn on Location Services Settings", "Settings", "Cancel");
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus && ischecked)
        {
            CheckStatus();
        }
    }
}
