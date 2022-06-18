using System;
using UnityEngine;
using UnityEngine.UI;

public class ListViewItem : MonoBehaviour
{
    public Toggle Toggle;
    public RectTransform View;
    public float MaxViewHeight;
    public bool UpdateParentLayout;
    public RectTransform Content;
    public ScrollRect ScrollRect;

    private float viewHeight;
    private RectTransform rectTransform;
    private float toggleHeight;
    private UpdateLayoutRuntime layout;

    void Start()
    {
        layout = transform.parent.GetComponent<UpdateLayoutRuntime>();
        rectTransform = GetComponent<RectTransform>();
        toggleHeight = Toggle.GetComponent<RectTransform>().sizeDelta.y;
        viewHeight = GetViewHeight();
        Vector2 viewSizeDelta = View.sizeDelta;
        viewSizeDelta.y = viewHeight;
        View.sizeDelta = viewSizeDelta;
        RefreshViewHeight();
        layout.UpdateLayout();
    }

    private void Update()
    {
        float newHeight = GetViewHeight();
        if (newHeight != viewHeight)
        {
            viewHeight = newHeight;
            layout.UpdateLayout();
        }
        if (View.sizeDelta.y != viewHeight)
        {
            Vector2 viewSizeDelta = View.sizeDelta;
            if (Math.Abs(viewSizeDelta.y - viewHeight) <= 1f)
            {
                viewSizeDelta.y = viewHeight;
            }
            else
            {
                viewSizeDelta.y += (viewHeight - View.sizeDelta.y) * 10 * Time.deltaTime;
            }
            View.sizeDelta = viewSizeDelta;
            RefreshViewHeight();
        }
    }

    private float GetViewHeight()
    {
        if (Toggle.isOn)
        {
            if (Content.sizeDelta.y > MaxViewHeight)
            {
                ScrollRect.enabled = true;
                return MaxViewHeight;
            }
            ScrollRect.enabled = false;
            return Content.sizeDelta.y;
        }
        return 0;
    }

    private void RefreshViewHeight()
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = toggleHeight + View.sizeDelta.y;
        rectTransform.sizeDelta = sizeDelta;
    }
}
