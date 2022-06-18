using System;
using TMPro;
using UnityEngine;

public class CheckedItem : MonoBehaviour
{
    public TextMeshProUGUI TextProvice;
    public event Action<string> OnItemClicked;

    private string province;

    public void UpdateData(string province)
    {
        TextProvice.text = province;
        this.province = province;
    }

    public void OnClick()
    {
        OnItemClicked?.Invoke(province);
    }
}
