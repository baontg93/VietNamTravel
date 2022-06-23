using System;
using TMPro;
using UnityEngine;

public class CheckedItem : MonoBehaviour
{
    public TextMeshProUGUI TextProvice;

    protected string province;

    public virtual void UpdateData(string province)
    {
        TextProvice.text = province;
        this.province = province;
    }

    public virtual void OnClick()
    {
        EventManager.Instance.Publish(GameEvent.FocusOnProvince, province);
    }
}
