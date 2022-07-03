using TMPro;
using UnityEngine;

public class ProvinceInfo : MonoBehaviour
{
    public TextMeshProUGUI AddressText;
    public TextMeshProUGUI InfomationText;
    public ScriptableProvince ScriptableProvince;


    public void ShowData(string provinceName, string unlockedTime)
    {
        gameObject.SetActive(true);
        Province province = ScriptableProvince.GetProvince(provinceName);
        AddressText.text = province.Name;
        string status = string.IsNullOrEmpty(unlockedTime) ? "chưa khám phá" : $"đã khám phá vào ngày\n{unlockedTime}";
        InfomationText.text = $"Thông tin:\n" +
            $"  - Cấp: {province.Type}\n" +
            $"  - Diện tích: {province.Area} km²\n" +
            $"  - Quận / huyện: {province.Districts.Count}\n" +
            $"\nTrạng thái: {status}";
    }

    // Update is called once per frame
    public void Clean()
    {
        gameObject.SetActive(false);
        AddressText.text = "";
        InfomationText.text = "";
    }
}
