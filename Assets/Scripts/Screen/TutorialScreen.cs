using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour
{
    public GameObject Content;
    public Button buttonFinished;

    void Start()
    {
        bool isFinished = PlayerPrefs.GetString("TutorialScreen_Finished", "false") == "true";
        Content.SetActive(!isFinished);
        if (!isFinished)
        {
            buttonFinished.onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("TutorialScreen_Finished", "true");
                Destroy(gameObject);
            });
        } else
        {
            Destroy(gameObject);
        }
    }
}