using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ByteUtils
{
    // Convert an object to a byte array
    public static byte[] Serialize(object obj)
    {
        if (obj == null)
            return null;

        BinaryFormatter bf = new();
        MemoryStream ms = new();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }

    // Convert a byte array to an Object
    public static T Deserialize<T>(byte[] arrBytes)
    {
        MemoryStream memStream = new();
        BinaryFormatter binForm = new();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        T obj = (T)binForm.Deserialize(memStream);

        return obj;
    }

    // Convert a byte array to an Unity Sprite
    public static Sprite ToSprite(byte[] imageData)
    {
        if (imageData.IsNullOrEmpty()) return null;

        GetImageSize(imageData, out int width, out int height);

        Texture2D texture = new(width, height, TextureFormat.ARGB32, false, true);
        texture.hideFlags = HideFlags.HideAndDontSave;
        texture.filterMode = FilterMode.Point;
        texture.LoadImage(imageData);

        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private static void GetImageSize(byte[] imageData, out int width, out int height)
    {
        width = ReadInt(imageData, 3 + 15);
        height = ReadInt(imageData, 3 + 15 + 2 + 2);
    }

    private static int ReadInt(byte[] imageData, int offset)
    {
        return (imageData[offset] << 8) | imageData[offset + 1];
    }
}