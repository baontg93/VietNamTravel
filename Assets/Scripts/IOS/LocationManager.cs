using System;
using TMPro;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public TextMeshProUGUI Address;
    string msgReceivingGameObjectName;
    string msgReceivingMethodName;

    string location;
    string address;

    public Action<string> OnLocation_Updated = delegate { };

    void Start()
    {
        msgReceivingGameObjectName = "LocationManager";
        msgReceivingMethodName = "SetLocationCallbackMessage";
        CheckStatus();
    }

    public void SetLocationCallbackMessage(string messageTemp)
    {
        string[] values = messageTemp.Split('/');
        if (values.Length > 1)
        {
            string finalMsg = "";
            if (values[0] == "Address")
            {
                for (int i = 1; i < values.Length; i++)
                {
                    finalMsg += values[i];
                }
                address = finalMsg;
                Address.text = address;
                OnLocation_Updated?.Invoke(address);
            }
            else if (values[0] == "Location")
            {
                for (int i = 1; i < values.Length; i++)
                {
                    finalMsg += values[i];
                }
                location = finalMsg;
            }
        }

    }

    void SetCallback()
    {
        LocationManagerBridge.setMessageReceivingObjectName(msgReceivingGameObjectName, msgReceivingMethodName);
    }

    public void CheckStatus()
    {
#if !UNITY_EDITOR
        int temp = LocationManagerBridge.getAuthrizationLevelForApplication();
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
                SetCallback();
                LocationManagerBridge.startLocationMonitoring();
                break;
            default:
                Debug.Log("Location OK");
                break;
        }
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
        if (focus)
        {
            CheckStatus();
        }
    }
}
