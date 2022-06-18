using System;
using System.Text;
using UnityEngine;
using VoxelBusters.EssentialKit;

public class MobileStorage : MonoBehaviour
{
    private static bool IsCloudAvailable()
    {
        bool isAvailable = CloudServices.IsAvailable();
        if (!isAvailable)
        {
            CloudServices.Synchronize();
        }
        return isAvailable;
    }

    public static void SetInt(string key, int value)
    {
        if (IsCloudAvailable())
        {
            CloudServices.SetInt(key, value);
        }

        PlayerPrefs.SetInt(key, value);
    }

    public static int GetInt(string key)
    {
        return GetInt(key, 0);
    }

    public static int GetInt(string key, int defaultValue)
    {
        if (IsCloudAvailable())
        {
            int value = CloudServices.GetInt(key);
            if (value != 0)
            {
                return value;
            }
        }

        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SetFloat(string key, float value)
    {
        if (IsCloudAvailable())
        {
            CloudServices.SetFloat(key, value);
        }

        PlayerPrefs.SetFloat(key, value);
    }

    public static float GetFloat(string key)
    {
        return GetFloat(key, 0f);
    }

    public static float GetFloat(string key, float defaultValue)
    {
        if (IsCloudAvailable())
        {
            float value = CloudServices.GetFloat(key);
            if (value != 0)
            {
                return value;
            }
        }

        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static void SetString(string key, string value)
    {
        if (IsCloudAvailable())
        {
            CloudServices.SetString(key, value);
        }

        PlayerPrefs.SetString(key, value);
    }

    public static string GetString(string key)
    {
        return GetString(key, "");
    }

    public static string GetString(string key, string defaultValue)
    {
        if (IsCloudAvailable())
        {
            string value = CloudServices.GetString(key);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
        }

        return PlayerPrefs.GetString(key, defaultValue);
    }

    public static void SetBool(string key, bool value)
    {
        SetString(key, value ? "true" : "false");
    }

    public static bool GetBool(string key)
    {
        return GetBool(key, false);
    }

    public static bool GetBool(string key, bool defaultValue)
    {
        string value = GetString(key);
        if (!string.IsNullOrEmpty(value))
        {
            return value == "true";
        }
        return defaultValue;
    }

    public static void SetSprite(string key, Sprite sprite)
    {
        if (sprite != null)
        {
            Texture2D tex = sprite.texture;
            Texture2D texCopy = new(tex.width, tex.height, tex.format, tex.mipmapCount > 1);
            texCopy.LoadRawTextureData(tex.GetRawTextureData());
            texCopy.Apply();
            byte[] arr = texCopy.EncodeToPNG();
            Destroy(texCopy);
            SetByteArray(key, arr);
        }
    }

    public static Sprite GetSprite(string key)
    {
        return GetSprite(key, null);
    }

    public static Sprite GetSprite(string key, Sprite defaultValue)
    {
        byte[] value = GetByteArray(key);

        if (value.IsNullOrEmpty())
        {
            return defaultValue;
        }

        return ByteUtils.ToSprite(value);
    }

    public static void SetObject(string key, object value)
    {
        if (value != null)
        {
            SetByteArray(key, ByteUtils.Serialize(value));
        }
    }

    public static T GetObject<T>(string key)
    {
        return GetObject<T>(key, default);
    }

    public static T GetObject<T>(string key, T defaultValue)
    {
        byte[] value = GetByteArray(key);

        if (value.IsNullOrEmpty())
        {
            return defaultValue;
        }

        return ByteUtils.Deserialize<T>(value);
    }

    public static void SetByteArray(string key, byte[] value)
    {
        if (IsCloudAvailable())
        {
            CloudServices.SetByteArray(key, value);
        }
        
        string data = Convert.ToBase64String(value);
        PlayerPrefs.SetString(key, data);
    }

    public static byte[] GetByteArray(string key)
    {
        return GetByteArray(key, null);
    }

    public static byte[] GetByteArray(string key, byte[] defaultValue)
    {
        if (IsCloudAvailable())
        {
            byte[] value = CloudServices.GetByteArray(key);
            if (value != null)
            {
                return value;
            }
        }

        string stringData = PlayerPrefs.GetString(key);
        if (!string.IsNullOrEmpty(stringData))
        {
            return Convert.FromBase64String(stringData);
        }

        return defaultValue;
    }

    public static void RemoveKey(string key)
    {
        if (IsCloudAvailable())
        {
            CloudServices.RemoveKey(key);
        }

        PlayerPrefs.HasKey(key);
    }

}
