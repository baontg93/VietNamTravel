using DG.Tweening;
using TMPro;

public class CongratScreen : BaseScreen
{
    public TextMeshProUGUI AddressText;

    private Tween tween;

    public override void Show()
    {
        base.Show();

        if (tween != null) tween.Kill();
        tween = DOVirtual.DelayedCall(5f, () =>
        {
            Hide();
        });
    }

    public void SetProvinceName(string province)
    {
        AddressText.text = province;
    }

    public override void Reset()
    {
        base.Reset();
        if (tween != null) tween.Kill();
        tween = null;
    }
}