using DG.Tweening;
using System;
using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    [SerializeField] protected bool playAnim = true;
    public GameObject Content;
    public event Action OnShown = delegate { };
    public event Action OnHiden = delegate { };

    public bool IsShown { get { return isShown; } }

    protected bool isShown = false;
    protected bool isRunningTween = false;

    protected const float START_SCALE = 1.1f;
    protected const float ANIM_DUR = 0.3f;

    public virtual void Start()
    {
        if (!isShown)
        {
            Hide();
        }
    }

    public virtual void Show()
    {
        Reset();
        isShown = true;

        gameObject.SetActive(true);
        Content.SetActive(true);

        if (playAnim)
        {
            Content.FadeIn(ANIM_DUR).SetEase(Ease.OutBack);
            Content.transform.DOScale(1, ANIM_DUR).SetEase(Ease.OutBack).OnComplete(() =>
            {
                OnShown?.Invoke();
            });
        }
        else
        {
            Content.transform.localScale = Vector3.one;
            OnShown?.Invoke();
        }

    }

    public virtual void Hide()
    {
        Hide(true);
    }
    public virtual void Hide(bool anim)
    {
        if (!isShown)
        {
            Reset();
            return;
        }

        if (isRunningTween)
        {
            return;
        }
        isShown = false;
        isRunningTween = true;

        if (anim && playAnim)
        {
            Content.FadeOut(ANIM_DUR).SetEase(Ease.InSine);
            Content.transform.DOScale(START_SCALE, ANIM_DUR).SetEase(Ease.InBack).OnComplete(() =>
            {
                Reset();
                OnHiden?.Invoke();
            });
        }
        else
        {
            Reset();
            OnHiden?.Invoke();
        }
    }

    public virtual void Reset()
    {
        isShown = false;
        isRunningTween = false;
        Content.SetActive(false);
        Content.transform.localScale = new Vector3(START_SCALE, START_SCALE);
        gameObject.SetActive(false);
    }
}
