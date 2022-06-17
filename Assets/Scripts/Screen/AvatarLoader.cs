using System;
using UnityEngine;

public class AvatarLoader : SingletonBehaviour<AvatarLoader>
{
    public Sprite[] ListAvatar;
    public MediaPicker mediaPicker;

    public Sprite GetAvatar(string name)
    {
        for (int i = 0; i < ListAvatar.Length; i++)
        {
            if (ListAvatar[i].name == name)
            {
                return ListAvatar[i];
            }
        }
        return null;
    }

    public void SelectAvatar(Action<Sprite> onComplete)
    {
        mediaPicker.SelectImageFromGallery(onComplete);
    }
}
