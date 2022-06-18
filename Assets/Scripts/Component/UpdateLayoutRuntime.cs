using System.Collections.Generic;
using UnityEngine;

public class UpdateLayoutRuntime : MonoBehaviour
{
    public float Spacing;
    private List<RectTransform> rectTFs;

    public void UpdateLayout()
    {
        rectTFs = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tf = transform.GetChild(i);
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