using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementItem: MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Text;
    public GameObject Hint;

    public void UpdateData(AchievementData data)
    {
        Image.sprite = data.Sprite;
        Text.text = data.Description;
    }

    private void OnMouseDown()
    {
        Hint.SetActive(true);
    }

    private void OnMouseUp()
    {
        Hint.SetActive(false);
    }
}