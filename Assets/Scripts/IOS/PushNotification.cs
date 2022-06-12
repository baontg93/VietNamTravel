using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public class PushNotification : MonoBehaviour
    {
        public static event Action<NotificationPermissionStatus> OnPermissionStatusChange = delegate { };
        public static event Action<NotificationSettings> OnSettingsUpdate = delegate { };
        public static event Action<INotification> OnNotificationReceived = delegate { };
        
        // Start is called before the first frame update
        void Start()
        {
            if (!NotificationServices.IsPermissionAvailable())
            {
                NotificationServices.RequestPermission(NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound | NotificationPermissionOptions.Badge | NotificationPermissionOptions.ProvidesAppNotificationSettings, callback: (result, error) =>
                {
                    HandlePermissionStatusChange(result.PermissionStatus);
                });
            }
        }

        void OnEnable()
        {
            NotificationServices.OnNotificationReceived += HandleNotificationReceived;
            NotificationServices.OnSettingsUpdate += HandleSettingsUpdate;

        }

        void OnDisable()
        {
            NotificationServices.OnNotificationReceived -= HandleNotificationReceived;
            NotificationServices.OnSettingsUpdate -= HandleSettingsUpdate;
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
}
