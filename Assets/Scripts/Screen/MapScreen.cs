using UnityEngine;

public class MapScreen : BaseScreen
{
    public GameObject Map;

    public override void Show()
    {
        base.Show();
        Map.SetActive(true);
    }

    public override void Hide()
    {
        base.Hide();
        Map.SetActive(false);
    }

    public override void Reset()
    {
        base.Reset();
        Map.SetActive(false);
    }
}
