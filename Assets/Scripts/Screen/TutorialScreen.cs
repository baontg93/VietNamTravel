using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour
{
    public GameObject Content;
    public Button buttonFinished;

    void Start()
    {
        bool isFinished = PlayerPrefs.GetString("TutorialScreen_Finished", "false") == "true";
        if (!isFinished)
        {
            Show();
            buttonFinished.onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("TutorialScreen_Finished", "true");
                Hide();
            });
        } else
        {
            Hide();
        }
    }

    public void Show()
    {
        Content.SetActive(true);
    }

    public void Hide()
    {
        Content.SetActive(false);
    }
}