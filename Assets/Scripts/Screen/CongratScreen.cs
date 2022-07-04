using DG.Tweening;
using TMPro;

public class CongratScreen : BaseScreen
{
    public TextMeshProUGUI AddressText;

    private Tween tween;

    public override void Show()
    {
        base.Show();

        InactiveScreen.Instance.Hide();

        if (tween != null) tween.Kill();
        tween = DOVirtual.DelayedCall(3f, () =>
        {
            Hide();
        });
    }

    public void Show(string province)
    {
        Show();
        AddressText.text = "You unlocked\n" + province + "\nPlay more and unlock more.";
    }

    public override void Reset()
    {
        base.Reset();
        if (tween != null) tween.Kill();
        tween = null;
    }
}