using System;
using UnityEngine;
using VoxelBusters.EssentialKit;

public class MediaPicker : MonoBehaviour
{
    [SerializeField] private bool ovalSelection = true;
    [SerializeField] private bool autoZoom = true;
    [SerializeField] private float minAspectRatio = 1f;
    [SerializeField] private float maxAspectRatio = 1f;

    public void SelectImageFromGallery(Action<Sprite> onComplete, int width = 128, int height = 128)
    {
        MediaServices.SelectImageFromGalleryWithUserPermision(true, (textureData, error) =>
        {
            if (error == null)
            {
                Debug.Log("Select image from gallery finished successfully.");
                onComplete(GetSprite(textureData.GetTexture(), width, height));
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

    public Sprite GetSprite(Texture2D texture, int width = 256, int height = 256)
    {
        if (texture == null)
        {
            return null;
        }
        texture = Resize(texture, width, height);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture aciveRT = RenderTexture.active;
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(targetX, targetY);
        result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
        result.Apply();
        RenderTexture.active = aciveRT;
        Destroy(rt);
        return result;
    }
}
