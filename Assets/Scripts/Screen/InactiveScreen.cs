using UnityEngine;

public class InactiveScreen : SingletonBehaviour<InactiveScreen>
{
    public GameObject Content;
    float activeDuration = 10;

    private void Awake()
    {
        Hide();
    }

    void FixedUpdate()
    {
        if (Input.touchCount >= 1)
        {
            Hide();
        }

        if (activeDuration <= 0)
        {
            Content.SetActive(true);
        } else
        {
            activeDuration -= Time.deltaTime;
        }
    }

    public void Hide()
    {
        activeDuration = 5;
        Content.SetActive(false);
    }
}
