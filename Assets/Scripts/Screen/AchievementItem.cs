using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementItem: MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Text;
    public GameObject Hint;

    private float delayHide = 0;
    private bool hiding = false;

    public void UpdateData(AchievementData data)
    {
        Image.sprite = data.Sprite;
        Text.text = data.Description;
        Hint.SetActive(false);
    }

    private void Start()
    {
        Button button = Image.GetOrAddComponent<Button>();
        button.RegisterEventTrigger(UnityEngine.EventSystems.EventTriggerType.PointerDown, OnPointerDown);
        button.RegisterEventTrigger(UnityEngine.EventSystems.EventTriggerType.PointerUp, OnPointerUp);
    }

    private void OnDisable()
    {
        hiding = false;
        Hint.SetActive(false);
    }

    private void OnPointerDown()
    {
        hiding = false;
        Hint.SetActive(true);
    }

    private void OnPointerUp()
    {
        hiding = true;
        delayHide = 0.5f;
    }

    private void Update()
    {
        if (hiding)
        {
            if (delayHide >= 0)
            {
                delayHide -= Time.deltaTime;
            } else
            {
                hiding = false;
                delayHide = 0.5f;
                Hint.SetActive(false);
            }
        }
    }
}