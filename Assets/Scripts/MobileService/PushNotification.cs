using System;
using UnityEngine;
using VoxelBusters.EssentialKit;

public class PushNotification : MonoBehaviour
{
    public static event Action<NotificationPermissionStatus> OnPermissionStatusChange = delegate { };
    public static event Action<NotificationSettings> OnSettingsUpdate = delegate { };
    public static event Action<INotification> OnNotificationReceived = delegate { };

    public void CheckPermission()
    {
        if (!VoxelBusters.EssentialKit.NotificationServices.IsPermissionAvailable())
        {
            VoxelBusters.EssentialKit.NotificationServices.RequestPermission(NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound | NotificationPermissionOptions.Badge | NotificationPermissionOptions.ProvidesAppNotificationSettings, callback: (result, error) =>
            {
                HandlePermissionStatusChange(result.PermissionStatus);
            });
        }
    }

    void OnEnable()
    {
        VoxelBusters.EssentialKit.NotificationServices.OnNotificationReceived += HandleNotificationReceived;
        VoxelBusters.EssentialKit.NotificationServices.OnSettingsUpdate += HandleSettingsUpdate;
    }

    void OnDisable()
    {
        VoxelBusters.EssentialKit.NotificationServices.OnNotificationReceived -= HandleNotificationReceived;
        VoxelBusters.EssentialKit.NotificationServices.OnSettingsUpdate -= HandleSettingsUpdate;
    }

    #region Private methods

    private void HandlePermissionStatusChange(NotificationPermissionStatus newStatus)
    {
        OnPermissionStatusChange?.Invoke(newStatus);
    }

    private void HandleNotificationReceived(NotificationServicesNotificationReceivedResult data)
    {
        Debug.Log(string.Format("{0} received.", data.Notification));
        OnNotificationReceived?.Invoke(data.Notification);
    }

    private void HandleSettingsUpdate(NotificationSettings settings)
    {
        Debug.Log(string.Format("Settings Update: {0}", settings));
        OnSettingsUpdate?.Invoke(settings);
    }

    #endregion
}

