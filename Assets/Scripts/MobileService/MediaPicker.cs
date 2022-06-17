using System;
using UnityEngine;
using VoxelBusters.EssentialKit;

public class MediaPicker : MonoBehaviour
{
    [SerializeField] private bool ovalSelection = true;
    [SerializeField] private bool autoZoom = true;
    [SerializeField] private float minAspectRatio = 1f;
    [SerializeField] private float maxAspectRatio = 1f;

    public void SelectImageFromGallery(Action<Sprite> onComplete)
    {
        MediaServices.SelectImageFromGalleryWithUserPermision(true, (textureData, error) =>
        {
            if (error == null)
            {
                Debug.Log("Select image from gallery finished successfully.");
                CropImage(textureData.GetTexture(), onComplete);
            }
            else
            {
                Debug.Log("Select image from gallery failed with error. Error: " + error);
                onComplete(null);
            }
        });
    }

    private void CropImage(Texture2D texture, Action<Sprite> onComplete)
    {
        ImageCropper.Instance.Show(texture, (bool result, Texture originalImage, Texture2D croppedImage) =>
        {
            // If screenshot was cropped successfully
            Sprite sprite = null;
            if (result)
            {
                sprite = GetSprite(croppedImage);
            }
            else if (texture != null)
            {
                sprite = GetSprite(texture);
            }
            onComplete(sprite);
        },
        settings: new ImageCropper.Settings()
        {
            ovalSelection = ovalSelection,
            autoZoomEnabled = autoZoom,
            imageBackground = Color.clear, // transparent background
            selectionMinAspectRatio = minAspectRatio,
            selectionMaxAspectRatio = maxAspectRatio

        },
        croppedImageResizePolicy: (ref int width, ref int height) =>
        {
            // uncomment lines below to save cropped image at half resolution
            //width /= 2;
            //height /= 2;
        });
    }

    Sprite GetSprite(Texture2D texture)
    {
        if (texture == null)
        {
            return null;
        }
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
