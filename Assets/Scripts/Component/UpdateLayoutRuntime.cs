using System.Collections.Generic;
using UnityEngine;

public class UpdateLayoutRuntime : MonoBehaviour
{
    public Transform Content;
    public float Spacing;
    private List<RectTransform> rectTFs;

    public void UpdateLayout()
    {
        if (Content == null) Content = transform;
        rectTFs = new();
        for (int i = 0; i < Content.childCount; i++)
        {
            Transform tf = Content.GetChild(i);
            if (tf.gameObject.activeSelf)
            {
                rectTFs.Add(tf as RectTransform);
            }
        }
    }

    private void LateUpdate()
    {
        UpdateChildren();
    }

    private void UpdateChildren()
    {
        if (rectTFs != null)
        {
            float height = 0;
            for (int i = 0; i < rectTFs.Count; i++)
            {
                Vector2 pos = rectTFs[i].localPosition;
                pos.y = height;
                rectTFs[i].localPosition = pos;
                height -= (rectTFs[i].sizeDelta.y + Spacing);
            }
        }
    }
}