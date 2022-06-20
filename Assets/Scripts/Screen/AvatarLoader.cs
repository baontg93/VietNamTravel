using System;
using UnityEngine;

public class AvatarLoader : SingletonBehaviour<AvatarLoader>
{
    public Sprite[] ListAvatar;
    public MediaPicker mediaPicker;

    public Sprite GetAvatar(int id)
    {
        string name = "avatar_" + id;
        Sprite sprite = null;
        for (int i = 0; i < ListAvatar.Length; i++)
        {
            if (ListAvatar[i].name == name)
            {
                sprite = ListAvatar[i];
            }
        }

        if (sprite)
        {
            // assume "sprite" is your Sprite object
            var croppedTexture = GetTexture(sprite);
            return mediaPicker.GetSprite(croppedTexture);
        }


        return sprite;
    }

    public void SelectAvatar(Action<Sprite> onComplete)
    {
        mediaPicker.SelectImageFromGallery(onComplete);
    }

    private Texture2D GetTexture(Sprite sprite)
    {
        try
        {
            Texture2D texture = new Texture2D(
    (int)sprite.textureRect.width,
    (int)sprite.textureRect.height
);

            Color[] pixels = sprite.texture.GetPixels(
                (int)sprite.textureRect.x,
                (int)sprite.textureRect.y,
                (int)sprite.textureRect.width,
                (int)sprite.textureRect.height
            );

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
        catch
        {
            return sprite.texture;
        }
    }
}
