using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class MobileCloudServices : SingletonBehaviour<MobileCloudServices>
{
    public static event Action<JoinGameData> OnJoinGame;
    public static event Action<string, object> OnDataReceived;

    private bool firstUpdate = true;

    private void Awake()
    {
        CloudServices.Synchronize();
    }

    protected void OnEnable()
    {
        // register for events
        CloudServices.OnUserChange += OnUserChange;
        CloudServices.OnSavedDataChange += OnSavedDataChange;
        CloudServices.OnSynchronizeComplete += OnSynchronizeComplete;
    }

    protected void OnDisable()
    {
        // unregister from events
        CloudServices.OnUserChange -= OnUserChange;
        CloudServices.OnSavedDataChange -= OnSavedDataChange;
        CloudServices.OnSynchronizeComplete -= OnSynchronizeComplete;
    }

    private void OnUserChange(CloudServicesUserChangeResult result, Error error)
    {
        var user = result.User;
        Debug.Log("Received user change callback.");
        Debug.Log("User id: " + user.UserId);
        Debug.Log("User status: " + user.AccountStatus);
        FirstUpdate();
    }

    private void OnSavedDataChange(CloudServicesSavedDataChangeResult result)
    {
        var changedKeys = result.ChangedKeys;
        Debug.Log("Received saved data change callback.");
        Debug.Log("Reason: " + result.ChangeReason);
        Debug.Log("Total changed keys: " + changedKeys.Length);
        Debug.Log("Here are the changed keys:");
        for (int iter = 0; iter < changedKeys.Length; iter++)
        {
            Debug.Log(string.Format("[{0}]: {1}", iter, changedKeys[iter]));
        }
    }

    private void OnSynchronizeComplete(CloudServicesSynchronizeResult result)
    {
        Debug.Log("Received synchronize finish callback.");
        Debug.Log("Status: " + result.Success);
        FirstUpdate();
    }

    private void FirstUpdate()
    {
        string username = MobileStorage.GetString(StogrageKey.USER_NAME, "No name");
        Sprite avatar = MobileStorage.GetSprite(StogrageKey.USER_AVATAR);
        UnlockedData unlockedData = MobileStorage.GetObject<UnlockedData>(StogrageKey.USER_UNLOCKED_DATA);
        if (firstUpdate)
        {
            firstUpdate = false;
            JoinGameData joinGameData = new();
            joinGameData.Username = username;
            if (avatar != null)
            {
                joinGameData.Avatar = avatar;
            }
            if (unlockedData != null)
            {
                joinGameData.UnlockedData = unlockedData;
            }
            OnJoinGame?.Invoke(joinGameData);
        } else
        {
            OnDataReceived(StogrageKey.USER_NAME, username);
            if (avatar != null)
            {
                OnDataReceived(StogrageKey.USER_AVATAR, avatar);
            }
        }
    }
}
