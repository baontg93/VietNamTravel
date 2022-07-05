using UnityEngine;

public class MapItem : MonoBehaviour
{
    public string Name;
    public string CheckedTime;
    public bool IsChecked = false;
    public Vector3 PosMainLane;
    public Vector3 PosCamView;

    public void Initialize(string name, Vector3 posMainLane, Vector3 posCamView)
    {
        Name = name;
        PosMainLane = posMainLane;
        PosCamView = posCamView;
    }

    public void SetChecked(bool isChecked, string checkedTime = "")
    {
        IsChecked = isChecked;
        CheckedTime = checkedTime;
    }
}
